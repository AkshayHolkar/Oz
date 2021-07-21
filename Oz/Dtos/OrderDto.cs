using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime DateCreation { get; set; }
        public string CustomerId { get; set; }
        public string OrderStatus { get; set; }
    }
}
