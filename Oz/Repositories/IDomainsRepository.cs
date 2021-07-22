using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public interface IDomainsRepository<T>
    {
        Task<ActionResult<IEnumerable<T>>> GetAllAsync();
        Task<ActionResult<T>> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<ActionResult<bool>> UpdateAsync(T entity);
        Task<ActionResult<bool>> DeleteAsync(int id);
        bool isExist(int id);
    }
}
