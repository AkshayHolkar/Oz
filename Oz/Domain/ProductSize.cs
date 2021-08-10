using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oz.Domain
{
    public class ProductSize
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(10)]
        public string ItemSize { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
