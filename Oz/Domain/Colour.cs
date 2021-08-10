using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oz.Domain
{
    public class Colour
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(7)]
        public string Color { get; set; }

        [Required]
        public int ProductQuantity { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
