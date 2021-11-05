using System.Threading.Tasks;

namespace Oz.Repositories.Contracts
{
    public interface IStringIdEntitiesMethods<T>
    {
        Task<T> GetByIdAsync(string id);
        Task DeleteAsync(string id);
        bool IsExist(string id);
    }
}
