using Microsoft.AspNetCore.Mvc;
using Oz.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public interface IColourRepository
    {
        Task<ActionResult<IEnumerable<ColourDto>>> GetAllAsync();
        Task<ActionResult<IEnumerable<ColourDto>>> GetAllProductColorsAsync(int productId);
        Task<ActionResult<ColourDto>> GetByIdAsync(int id);
        Task<ColourDto> CreateAsync(PostColourDto postColourDto);
        Task<ActionResult<bool>> UpdateAsync(ColourDto colourDto);
        Task<ActionResult<bool>> DeleteAsync(int id);
        bool IsExist(int id);
    }
}
