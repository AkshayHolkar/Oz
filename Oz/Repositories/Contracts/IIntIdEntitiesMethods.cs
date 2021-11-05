using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Repositories.Contracts
{
    public interface IIntIdEntitiesMethods<T>
    {
        Task<T> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        bool IsExist(int id);
    }
}
