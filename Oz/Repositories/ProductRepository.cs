using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Dtos;
using Oz.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            return await _context.Products.Select(product => product.AsDto()).ToListAsync();
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return null;

            return product.AsDto();
        }

        public async Task<ProductDto> CreateAsync(PostProductDto postProductDto)
        {
            var product = postProductDto.AsProductFromPostProductDto();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.AsDto();
        }

        public async Task UpdateAsync(ProductDto productDto)
        {
            _context.Entry(productDto.AsProductFromProductDto()).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public bool IsExist(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
