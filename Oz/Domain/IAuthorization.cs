using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Domain
{
    public interface IAuthorization
    {
        string CustomerId { get; set; }
    }
}
