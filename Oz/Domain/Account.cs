using System;
using System.ComponentModel.DataAnnotations;

namespace Oz.Domain
{
    public class Account
    {
        public string UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ContactName { get; set; }

        [Required]
        [MaxLength(100)]
        public string BusinessName { get; set; }
        public int ABN { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(100)]
        public string StreetAddress { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [MaxLength(50)]
        public string State { get; set; }

        [Required]
        [Range(200, 9999)]
        public int Postcode { get; set; }

        [Required]
        [MaxLength(50)]
        public string Country { get; set; }
        public bool Approved { get; set; } = false;
        public ApplicationUser User { get; set; }
    }
}
