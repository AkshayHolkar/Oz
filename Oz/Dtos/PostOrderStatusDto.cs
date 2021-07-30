using System.ComponentModel.DataAnnotations;

namespace Oz.Dtos
{
    public class PostOrderStatusDto
    {
        [Required]
        public string Name { get; set; }
    }
}
