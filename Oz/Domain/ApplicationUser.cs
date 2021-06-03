using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Account Account { get; set; }
    }
}
