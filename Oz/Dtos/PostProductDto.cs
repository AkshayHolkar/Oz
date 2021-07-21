using System.ComponentModel.DataAnnotations;

namespace Oz.Dtos
{
    public class PostProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public bool SizeNotApplicable { get; set; }
        [Required]
        public bool ColorNotApplicable { get; set; }
    }
}
