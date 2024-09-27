using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class OrderController(IShoppingCartService cartService, IUnitOfWork unit) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto orderDto) 
        {
            var email = User.GetEmail();
            var cart = await cartService.GetCartAsync(orderDto.CartId);
            if (cart == null) return BadRequest("Cart not found");
            if (cart.PaymentIntentId == null) return BadRequest("No payment intent for this order");
            var items = new List<OrderItem>();
            foreach (var item in cart.Items!)
            {
                var productItem = await unit.Repository<Product>().GetByIdAsync(item.Id);
                if (productItem == null) return BadRequest("Problem with the order");

                var itemOrdered = new ProductItemOrdered {
                    Id = item.Id,
                    Name = item.Name,
                    PictureUrl = item.PictureUrl
                };

                var orderItem = new OrderItem 
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };
                items.Add(orderItem);
            }
            var deliveryMethod = await unit.Repository<DeliveryMethod>().GetByIdAsync(orderDto.DeliveryMethodId);

            if (deliveryMethod == null) return BadRequest("No delivery method selected");

            var order = new Order {
                OrderItems = items,
                DeliveryMethod = deliveryMethod,
                ShippingAddress = orderDto.ShippingAddress,
                Subtotal = items.Sum(x => x.Price * x.Quantity),
                PaymentIntentId = cart.PaymentIntentId,
                PaymentSummary = orderDto.PaymentSummary,
                BuyerEmail = email
            };

            unit.Repository<Order>().Add(order);

            if (await unit.Complete()) 
            {
                return order;
            }
            return BadRequest("Problem creating order");
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser() 
        {
            var spec = new OrderSpecification(User.GetEmail());
            var orders = await unit.Repository<Order>().ListAsync(spec);
            var ordersReturn = orders.Select(o => o.ToDto()).ToList();
            return Ok(ordersReturn);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id) {
            var spec = new OrderSpecification(User.GetEmail(),id);
            var order = await unit.Repository<Order>().GetWithSpec(spec);
            if (order == null) return NotFound();
            return Ok(order.ToDto());
        }
    }
}