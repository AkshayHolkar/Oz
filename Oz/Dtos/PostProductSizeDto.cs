using System.ComponentModel.DataAnnotations;

namespace Oz.Dtos
{
    public class PostProductSizeDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string ItemSize { get; set; }
    }
}
