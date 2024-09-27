using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate
{
    public class ProductItemOrdered
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string PictureUrl { get; set; }  
    }
}