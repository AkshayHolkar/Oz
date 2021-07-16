using Oz.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Services
{
    public class SharedService : ISharedService
    {
        public bool UserOwnsDomain(IAuthorization domain, string userId)
        {
            return domain.CustomerId == userId;
        }
    }
}
