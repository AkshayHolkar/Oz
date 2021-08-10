using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oz.Domain
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
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

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
    }
}
