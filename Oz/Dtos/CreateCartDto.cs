using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Dtos
{
    public class CreateCartDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int MaxLimit { get; set; }
        [Required]
        public double Price { get; set; }

    }
}
