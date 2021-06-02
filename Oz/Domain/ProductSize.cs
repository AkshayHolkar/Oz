using System.ComponentModel.DataAnnotations.Schema;

namespace Oz.Domain
{
    public class ProductSize
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ItemSize { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
