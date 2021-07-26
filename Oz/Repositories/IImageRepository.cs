using Oz.Domain;
using Oz.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public interface IImageRepository
    {
        Task<List<ImageDto>> GetAllAsync();
        Task<List<ImageDto>> GetAllProductImagesAsync(int productId);
        Task<ImageDto> GetByIdAsync(int id);
        Task<ImageDto> GetMainImageAsync(int productId);
        Task<ImageDto> CreateAsync(Image image);
        Task UpdateAsync(PutImageDto putImageDto);
        Task DeleteAsync(int id);
        bool IsExist(int id);
        bool IsMainImageExist(int id);
    }
}
