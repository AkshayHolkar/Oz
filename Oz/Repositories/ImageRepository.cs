using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using Oz.Repositories.Contracts;
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

        public async Task<List<Image>> GetAllAsync()
        {
            return await _context.Images.ToListAsync();
        }

        public async Task<List<Image>> GetAllProductImagesAsync(int productId)
        {
            return await _context.Images
            .Where(i => i.ProductId == productId).ToListAsync();
        }

        public async Task<Image> GetByIdAsync(int id)
        {
            return await _context.Images.Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Image> GetMainImageAsync(int productId)
        {
            return await _context.Images.Where(i => i.Main == true && i.ProductId == productId).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Image image)
        {
            _context.Entry(image).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Image> CreateAsync(Image image)
        {
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
            return image;
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
