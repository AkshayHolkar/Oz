using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oz.Domain
{
    public class Cart
    {
        public int Id { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [Required]
        [MaxLength(300)]
        public string ImageUrl { get; set; }

        [MaxLength(7)]
        public string Color { get; set; }

        [MaxLength(10)]
        public string Size { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int MaxLimit { get; set; }

        [Required]
        public double Price { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public IdentityUser User { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
