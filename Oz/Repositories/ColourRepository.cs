using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using Oz.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public class ColourRepository : IColourRepository
    {
        private readonly DataContext _context;

        public ColourRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Colour>> GetAllAsync()
        {
            return await _context.Colours.ToListAsync();
        }

        public async Task<List<Colour>> GetAllProductColoursAsync(int productId)
        {
            return await _context.Colours.Where(i => i.ProductId == productId).ToListAsync();
        }

        public async Task<Colour> GetByIdAsync(int id)
        {
            var colour = await _context.Colours.FindAsync(id);
            return colour;
        }

        public async Task UpdateAsync(Colour colour)
        {
            _context.Entry(colour).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Colour> CreateAsync(Colour colour)
        {
            _context.Colours.Add(colour);
            await _context.SaveChangesAsync();
            return colour;
        }

        public async Task DeleteAsync(int id)
        {
            var colour = await _context.Colours.FindAsync(id);
            _context.Colours.Remove(colour);
            await _context.SaveChangesAsync();
        }

        public bool IsExist(int id)
        {
            return _context.Colours.Any(e => e.Id == id);
        }
    }
}
