using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public class SizeRepository : IDomainsRepository<Size>
    {
        private readonly DataContext _context;

        public SizeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Size>>> GetAllAsync()
        {
            return await _context.Sizes.ToListAsync();
        }

        public async Task<ActionResult<Size>> GetByIdAsync(int id)
        {
            return await _context.Sizes.FindAsync(id);
        }

        public async Task<Size> CreateAsync(Size entity)
        {
            _context.Sizes.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<ActionResult<bool>> UpdateAsync(Size entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ActionResult<bool>> DeleteAsync(int id)
        {
            var size = await _context.Sizes.FindAsync(id);
            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool isExist(int id)
        {
            return _context.Sizes.Any(e => e.Id == id);
        }
    }
}
