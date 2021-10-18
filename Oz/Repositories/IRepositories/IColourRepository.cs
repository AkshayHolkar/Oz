using Microsoft.AspNetCore.Mvc;
using Oz.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public interface IColourRepository
    {
        Task<List<ColourDto>> GetAllAsync();
        Task<List<ColourDto>> GetAllProductColorsAsync(int productId);
        Task<ColourDto> GetByIdAsync(int id);
        Task<ColourDto> CreateAsync(PostColourDto postColourDto);
        Task UpdateAsync(ColourDto colourDto);
        Task DeleteAsync(int id);
        bool IsExist(int id);
    }
}
