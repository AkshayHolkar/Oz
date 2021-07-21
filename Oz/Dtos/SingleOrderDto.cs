using Oz.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Dtos
{
    public class SingleOrderDto
    {
        public int Id { get; set; }
        public DateTime DateCreation { get; set; }
        public string CustomerId { get; set; }
        public string OrderStatus { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
