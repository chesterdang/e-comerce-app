using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;
using Core.Interfaces;

namespace Core.Specifications
{
    public class OrderSpecification : Specification<Order>
    {
        public OrderSpecification(string email) : base(x => x.BuyerEmail == email) 
        {
            AddInclude(x => x.OrderItems);
            AddInclude(x => x.DeliveryMethod);
            AddOrerBy(x => x.OrderDate);
        }
        public OrderSpecification(string email, int id) : base(x => x.BuyerEmail == email && x.Id == id)
        {
            AddInclude("OrderItems");
            AddInclude("DeliveryMethod");
            AddOrerBy(x => x.OrderDate);
        }

        public OrderSpecification(string paymentIntentId, bool isPaymentIntent): 
            base(x => x.PaymentIntentId == paymentIntentId)
        {
            AddInclude("OrderItems");
            AddInclude("DeliveryMethod");
        }

        public OrderSpecification(OrderSpecParams specParams) : base( x => 
            string.IsNullOrEmpty(specParams.Status) || x.Status == ParseStatus(specParams.Status)
        ) 
        {
            AddInclude("OrderItems");
            AddInclude("DeliveryMethod");
            ApplyPaging(specParams.PageSize , specParams.PageSize * (specParams.PageIndex-1));
            AddOrerByDescending(x => x.OrderDate);

        }

        public OrderSpecification(int id) : base (x => x.Id == id)
        {
            AddInclude("OrderItems");
            AddInclude("DeliveryMethod");

        }


        private static OrderStatus? ParseStatus(string status) {
            if (Enum.TryParse<OrderStatus>(status, true, out var result)) return result;
            return null;
        }
    }
}