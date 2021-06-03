using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DateCreation { get; set; }
        public string CustomerId { get; set; }
        public string OrderStatus { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public IdentityUser User { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
