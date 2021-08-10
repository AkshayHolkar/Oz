using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oz.Domain
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public DateTime DateCreation { get; set; }

        public string CustomerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string OrderStatus { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public IdentityUser User { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
