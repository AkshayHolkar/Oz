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
        Task<List<CartDto>> GetAllAsync(string userId);
        Task<CartDto> GetByIdAsync(int id);
        Task<CartDto> CreateAsync(Cart cart);
        Task UpdateAsync(CartDto cartDto);
        Task DeleteAsync(int id);
        bool IsExist(int id);
    }
}
