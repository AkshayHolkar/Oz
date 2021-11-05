using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Repositories.Contracts
{
    public interface IGetAllAsyncWithoutParameter<T>
    {
        Task<List<T>> GetAllAsync();
    }
}
