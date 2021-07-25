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
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(PostProductDto postProductDto);
        Task UpdateAsync(ProductDto productDto);
        Task DeleteAsync(int id);
        bool IsExist(int id);
    }
}
