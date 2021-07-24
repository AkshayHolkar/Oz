using Microsoft.AspNetCore.Mvc;
using Oz.Domain;
using Oz.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public interface ICartRepository
    {
        Task<ActionResult<IEnumerable<CartDto>>> GetAllAsync(string userId);
        Task<ActionResult<CartDto>> GetByIdAsync(int id);
        Task<CartDto> CreateAsync(Cart cart);
        Task<ActionResult<bool>> UpdateAsync(CartDto cartDto);
        Task<ActionResult<bool>> DeleteAsync(int id);
        bool IsExist(int id);
    }
}
