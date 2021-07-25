using Oz.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public interface IProductSizeRepository
    {
        Task<List<ProductSizeDto>> GetAllAsync();
        Task<List<ProductSizeDto>> GetAllProductSizesByProductIdAsync(int productId);
        Task<ProductSizeDto> GetByIdAsync(int id);
        Task<ProductSizeDto> CreateAsync(PostProductSizeDto postProductSizeDto);
        Task UpdateAsync(ProductSizeDto productSizeDto);
        Task DeleteAsync(int id);
        bool IsExist(int id);
    }
}
