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
    public class ImageRepository : IImageRepository
    {
        private readonly DataContext _context;

        public ImageRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<ImageDto>> GetAllAsync()
        {
            return await _context.Images.Select(x => new ImageDto()
            {
                Id = x.Id,
                Name = x.Name,
                Main = x.Main,
                ProductId = x.ProductId,
            }).ToListAsync();
        }

        public async Task<List<ImageDto>> GetAllProductImagesAsync(int productId)
        {
            return await _context.Images.Select(x => new ImageDto()
            {
                Id = x.Id,
                Name = x.Name,
                Main = x.Main,
                ProductId = x.ProductId,
            })
            .Where(i => i.ProductId == productId).ToListAsync();
        }

        public async Task<ImageDto> GetByIdAsync(int id)
        {
            return await _context.Images.Where(i => i.Id == id).Select(x => new ImageDto()
            {
                Id = x.Id,
                Name = x.Name,
                Main = x.Main,
                ProductId = x.ProductId,
            }).FirstOrDefaultAsync();
        }

        public async Task<ImageDto> GetMainImageAsync(int productId)
        {
            return await _context.Images.Where(i => i.Main == true && i.ProductId == productId).Select(x => new ImageDto()
            {
                Id = x.Id,
                Name = x.Name,
                Main = x.Main,
                ProductId = x.ProductId,
            }).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(PutImageDto putImageDto)
        {
            _context.Entry(putImageDto.AsImageFromPutImageDto()).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<ImageDto> CreateAsync(Image image)
        {
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
            return image.AsDto();
        }

        public async Task DeleteAsync(int id)
        {
            var image = await _context.Images.FindAsync(id);
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }

        public bool IsExist(int id)
        {
            return _context.Images.Any(e => e.Id == id);
        }

        public bool IsMainImageExist(int productId)
        {
            return _context.Images.Any(e => e.ProductId == productId && e.Main == true);
        }
    }
}
