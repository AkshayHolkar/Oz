using System.Collections.Generic;
using System.Threading.Tasks;

namespace Oz.Repositories.Contracts
{
    public interface IAsyncRepository<T>
    {
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
    }
}
