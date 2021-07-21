using System.ComponentModel.DataAnnotations;

namespace Oz.Dtos
{
    public class PostOrderDetailDto
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
