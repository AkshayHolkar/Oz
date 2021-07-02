using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Domain
{
    public class Account
    {
        public string UserId { get; set; }
        public string ContactName { get; set; }
        public string BusinessName { get; set; }
        public int ABN { get; set; }
        public string Phone { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Postcode { get; set; }
        public string Country { get; set; }
        public bool Approved { get; set; } = false;
        public ApplicationUser User { get; set; }
    }
}
