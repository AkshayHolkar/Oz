using System.ComponentModel.DataAnnotations;

namespace Oz.Dtos
{
    public class PostSizeDto
    {
        [Required]
        public string Name { get; set; }
    }
}
