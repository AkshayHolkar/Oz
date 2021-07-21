using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using Oz.Dtos;
using Oz.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Controllers.V1
{
    [Route("api/v1/Images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImagesController(DataContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: api/v1/Images
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageDto>>> GetImages([FromQuery] int productId)
        {
            if (productId != 0)
                return await _context.Images.Select(x => new ImageDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Main = x.Main,
                    ProductId = x.ProductId,
                    ImageScr = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.Name)
                })
                .Where(i => i.ProductId == productId).ToListAsync();

            return await _context.Images.Select(x => new ImageDto()
            {
                Id = x.Id,
                Name = x.Name,
                Main = x.Main,
                ProductId = x.ProductId,
                ImageScr = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.Name)
            }).ToListAsync();
        }

        // GET: api/v1/Images/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageDto>> GetImage(int id)
        {
            var image = await _context.Images.Where(i => i.Main == true && i.ProductId == id).Select(x => new ImageDto()
            {
                Id = x.Id,
                Name = x.Name,
                Main = x.Main,
                ProductId = x.ProductId,
                ImageScr = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.Name)
            }).FirstOrDefaultAsync();

            if (image == null)
            {
                return NotFound();
            }

            return image;
        }

        // PUT: api/v1/Images/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage(int id, [FromBody] PutImageDto putImageDto)
        {
            if (id != putImageDto.Id)
            {
                return BadRequest();
            }

            _context.Entry(putImageDto.AsImageFromPutImageDto()).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/v1/Images
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ImageDto>> PostImage(IFormCollection data, IFormFile imageFile)
        {
            Image image = new Image();
            image.ProductId = Int32.Parse(data["productId"]);
            image.Main = bool.Parse(data["main"]);
            image.Name = await SaveImage(imageFile);
            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImage), new { id = image.Id }, image.AsDto());
        }

        // DELETE: api/v1/Images/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            DeleteImage(image.Name);

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.Id == id);
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot/Images", imageName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
    }
}
