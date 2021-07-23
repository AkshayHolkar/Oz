using Microsoft.AspNetCore.Mvc;
using Oz.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public interface IProductRepository
    {
        Task<ActionResult<IEnumerable<ProductDto>>> GetAllAsync();
        Task<ActionResult<ProductDto>> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(PostProductDto postProductDto);
        Task<ActionResult<bool>> UpdateAsync(ProductDto productDto);
        Task<ActionResult<bool>> DeleteAsync(int id);
        bool IsExist(int id);
    }
}
