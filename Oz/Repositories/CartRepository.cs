using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using Oz.Dtos;
using Oz.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DataContext _context;

        public CartRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<CartDto>> GetAllAsync(string userId)
        {
            return await _context.Carts.Where(i => i.CustomerId == userId).Select(cart => cart.AsDto()).ToListAsync();
        }

        public async Task<CartDto> GetByIdAsync(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            return cart.AsDto();
        }

        public async Task<CartDto> CreateAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return cart.AsDto();
        }

        public async Task UpdateAsync(CartDto cartDto)
        {
            _context.Entry(cartDto.AsCartFromCartDto()).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public bool IsExist(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
