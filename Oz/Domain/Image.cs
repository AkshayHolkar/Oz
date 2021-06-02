using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oz.Domain
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Main { get; set; }
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [NotMapped]
        public string ImageScr { get; set; }
    }
}
