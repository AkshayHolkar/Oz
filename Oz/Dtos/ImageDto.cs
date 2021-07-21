using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Dtos
{
    public class ImageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Main { get; set; }
        public int ProductId { get; set; }
        public string ImageScr { get; set; }
    }
}
