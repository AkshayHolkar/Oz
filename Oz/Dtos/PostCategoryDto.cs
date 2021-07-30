using System.ComponentModel.DataAnnotations;

namespace Oz.Dtos
{
    public class PostCategoryDto
    {
        [Required]
        public string Name { get; set; }
    }
}
