using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Dtos
{
    public class ColourDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Color { get; set; }
        public int ProductQuantity { get; set; }
    }
}
