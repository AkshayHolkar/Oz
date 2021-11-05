using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using Oz.Repositories.Contracts;
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

        public async Task<List<Cart>> GetAllAsync(string userId)
        {
            return await _context.Carts.Where(i => i.CustomerId == userId).ToListAsync();
        }

        public async Task<Cart> GetByIdAsync(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            return cart;
        }

        public async Task<Cart> CreateAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return cart;
        }

        public async Task UpdateAsync(Cart cart)
        {
            _context.Entry(cart).State = EntityState.Modified;
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
