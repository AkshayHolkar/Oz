using System.ComponentModel.DataAnnotations;

namespace Oz.Dtos
{
    public class PostImageDto
    {
        [Required]
        public bool Main { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}
