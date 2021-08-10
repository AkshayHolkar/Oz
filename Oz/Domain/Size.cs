using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Domain
{
    public class Size
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Name { get; set; }
    }
}
