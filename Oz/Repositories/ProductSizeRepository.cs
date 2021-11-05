using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using Oz.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public class ProductSizeRepository : IProductSizeRepository
    {
        private readonly DataContext _context;

        public ProductSizeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<ProductSize>> GetAllAsync()
        {
            return await _context.ProductSizes.ToListAsync();
        }

        public async Task<List<ProductSize>> GetAllProductSizesByProductIdAsync(int productId)
        {
            return await _context.ProductSizes.Where(i => i.ProductId == productId).ToListAsync();
        }

        public async Task<ProductSize> GetByIdAsync(int id)
        {
            var productSize = await _context.ProductSizes.FindAsync(id);
            return productSize;
        }

        public async Task UpdateAsync(ProductSize productSize)
        {
            _context.Entry(productSize).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<ProductSize> CreateAsync(ProductSize productSize)
        {
            _context.ProductSizes.Add(productSize);
            await _context.SaveChangesAsync();
            return productSize;
        }

        public async Task DeleteAsync(int id)
        {
            var productSize = await _context.ProductSizes.FindAsync(id);
            _context.ProductSizes.Remove(productSize);
            await _context.SaveChangesAsync();
        }

        public bool IsExist(int id)
        {
            return _context.ProductSizes.Any(e => e.Id == id);
        }
    }
}
