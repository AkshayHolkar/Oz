using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Dtos;
using Oz.Extensions;
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

        public async Task<List<ProductSizeDto>> GetAllAsync()
        {
            return await _context.ProductSizes.Select(product => product.AsDto()).ToListAsync();
        }

        public async Task<List<ProductSizeDto>> GetAllProductSizesByProductIdAsync(int productId)
        {
            return await _context.ProductSizes.Where(i => i.ProductId == productId).Select(product => product.AsDto()).ToListAsync();
        }

        public async Task<ProductSizeDto> GetByIdAsync(int id)
        {
            var productSize = await _context.ProductSizes.FindAsync(id);
            return productSize.AsDto();
        }

        public async Task UpdateAsync(ProductSizeDto productSizeDto)
        {
            _context.Entry(productSizeDto.AsProductSizeFromProductSizeDto()).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<ProductSizeDto> CreateAsync(PostProductSizeDto postProductSizeDto)
        {
            var productSize = postProductSizeDto.AsProductSizeFromPostProductSizeDto();
            _context.ProductSizes.Add(productSize);
            await _context.SaveChangesAsync();
            return productSize.AsDto();
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
