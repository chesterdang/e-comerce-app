using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using API.SignalR;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage;
using Stripe;

namespace API.Controllers
{
    public class PaymentController(IPaymentService paymentService,
        IUnitOfWork unit, ILogger<PaymentController> logger, 
        IConfiguration config, IHubContext<NotificationHub> hubContext) : BaseApiController
    {
        private readonly string _whSecrect = config["StripeSettings:WhSecret"]!;

        [Authorize]
        [HttpPost("{cartId}")]
        public async Task<ActionResult<ShoppingCart>> CreateOrUpdatePaymentIntent(string cartId) 
        {
            var cart = await paymentService.CreateOrUpdatePaymentIntent(cartId);
            if (cart == null) return BadRequest("Problem with your cart");
            return Ok(cart);
        }

        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods () {
            return Ok(await unit.Repository<DeliveryMethod>().GetAllAsync());
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook() {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            try {
                var stripeEvent = ConstrucStripeEvent(json);
                if (stripeEvent.Data.Object is not PaymentIntent intent) 
                {
                    return BadRequest("Invalid event data");
                } 
                await HandlePaymentIntentSucceeded(intent);
                return Ok();
            
            }
                catch (StripeException ex) 
            {
                logger.LogError(ex, "Stripe webhook error");
                return StatusCode(StatusCodes.Status500InternalServerError,"Stripe webhook error");
            }
                catch (Exception ex) 
            {
                logger.LogError(ex, "An unexpected error occureed");
                return StatusCode(StatusCodes.Status500InternalServerError,"An unexpected error occureed");
            }
        }

        private async Task HandlePaymentIntentSucceeded(PaymentIntent intent)
        {
            if (intent.Status == "succeeded") 
            {
                var spec = new OrderSpecification(intent.Id, true);
                var order = await unit.Repository<Order>().GetWithSpec(spec)
                    ?? throw new Exception("Order not found");
                if ((long)order.GetTotal() * 100 != intent.Amount)
                {
                    order.Status = OrderStatus.PaymentMismatch;
                } 
                else 
                {
                    order.Status = OrderStatus.PaymentReceived;
                }
                await unit.Complete();
                // SignalR
                var connectionId = NotificationHub.GetConnectionIdByEmail(order.BuyerEmail);
                if (!string.IsNullOrEmpty(connectionId)) {
                    await hubContext.Clients.Client(connectionId)
                        .SendAsync("OrderCompleteNotification", order.ToDto());
                }
            }   
        }

        private Event ConstrucStripeEvent(string json)
        {
            try {
                return EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"],
                    _whSecrect);
            } catch (Exception ex)
            {
                logger.LogError(ex, "Failed to construct stripe event");
                throw new StripeException("Invalid signature");
            }
        }
    }
}