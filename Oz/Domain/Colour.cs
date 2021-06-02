using System.ComponentModel.DataAnnotations.Schema;

namespace Oz.Domain
{
    public class Colour
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Color { get; set; }

        public int ProductQuantity { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
