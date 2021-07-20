using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Dtos
{
    public class CreateAccountDto
    {
        [Required]
        public string ContactName { get; set; }
        [Required]
        public string BusinessName { get; set; }
        [Required]
        public int ABN { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public int Postcode { get; set; }
    }
}
