using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Dtos
{
    public class PutImageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Main { get; set; }
        public int ProductId { get; set; }
    }
}
