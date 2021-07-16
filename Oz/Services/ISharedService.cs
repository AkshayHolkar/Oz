using Oz.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Services
{
    public interface ISharedService
    {
        bool UserOwnsDomain(IAuthorization domain, string userId);
    }
}
