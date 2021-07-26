using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oz.Domain;
using Oz.Dtos;
using Oz.Repositories;
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
        private readonly IImageRepository _repository;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImagesController(IImageRepository repository, IWebHostEnvironment hostEnvironment)
        {
            _repository = repository;
            _hostEnvironment = hostEnvironment;
        }

        // GET: api/v1/Images
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageDto>>> GetImages([FromQuery] int productId)
        {
            if (productId != 0)
                return GetAllImagesWithImageSrc(await _repository.GetAllProductImagesAsync(productId));
            return GetAllImagesWithImageSrc(await _repository.GetAllAsync());
        }

        // GET: api/v1/Images/5
        [HttpGet("{productId}")]
        public async Task<ActionResult<ImageDto>> GetImage(int productId)
        {
            if (!_repository.IsMainImageExist(productId))
            {
                return NotFound();
            }
            return GetImageWithImageSrc(await _repository.GetMainImageAsync(productId));
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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_repository.IsExist(id))
            {
                return NotFound();
            }

            await _repository.UpdateAsync(putImageDto);

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

            var imageDto = await _repository.CreateAsync(image);

            return CreatedAtAction(nameof(GetImage), new { productId = imageDto.Id }, GetImageWithImageSrc(imageDto));
        }

        // DELETE: api/v1/Images/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            if (!_repository.IsExist(id))
            {
                return NotFound();
            }
            var image = await _repository.GetByIdAsync(id);
            DeleteImage(image.Name);
            await _repository.DeleteAsync(id);

            return NoContent();
        }

        private List<ImageDto> GetAllImagesWithImageSrc(List<ImageDto> imageDtoList)
        {
            List<ImageDto> images = new List<ImageDto>();
            foreach (var image in imageDtoList)
            {
                images.Add(GetImageWithImageSrc(image));
            }
            return images;
        }

        private ImageDto GetImageWithImageSrc(ImageDto image)
        {
            image.ImageScr = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, image.Name);
            return image;
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
