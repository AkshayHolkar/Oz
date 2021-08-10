using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oz.Domain
{
    public class OrderDetail
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [MaxLength(7)]
        public string Color { get; set; }

        [MaxLength(10)]
        public string Size { get; set; }

        [Required]
        public double TotalPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
