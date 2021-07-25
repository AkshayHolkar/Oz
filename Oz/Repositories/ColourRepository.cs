using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Dtos;
using Oz.Extensions;
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

        public async Task<List<ColourDto>> GetAllAsync()
        {
            return await _context.Colours.Select(colour => colour.AsDto()).ToListAsync();
        }

        public async Task<List<ColourDto>> GetAllProductColorsAsync(int productId)
        {
            return await _context.Colours.Where(i => i.ProductId == productId).Select(colour => colour.AsDto()).ToListAsync();
        }

        public async Task<ColourDto> GetByIdAsync(int id)
        {
            var colour = await _context.Colours.FindAsync(id);
            return colour.AsDto();
        }

        public async Task UpdateAsync(ColourDto colourDto)
        {
            _context.Entry(colourDto.AsColourFromColourDto()).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<ColourDto> CreateAsync(PostColourDto postColourDto)
        {
            var colour = postColourDto.AsCartFromPostColourDto();
            _context.Colours.Add(colour);
            await _context.SaveChangesAsync();
            return colour.AsDto();
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
