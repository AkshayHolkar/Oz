using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oz.Dtos;
using Oz.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Oz.Controllers.V1
{
    [Route("api/v1/ProductSizes")]
    [ApiController]
    public class ProductSizesController : ControllerBase
    {
        private readonly IProductSizeRepository _repository;

        public ProductSizesController(IProductSizeRepository repository)
        {
            _repository = repository;
        }

        // GET: api/v1/ProductSizes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductSizeDto>>> GetProductSizes([FromQuery] int productId = 0)
        {
            if (productId != 0)
                return await _repository.GetAllProductSizesByProductIdAsync(productId);
            return await _repository.GetAllAsync();
        }

        // GET: api/v1/ProductSizes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductSizeDto>> GetProductSize(int id)
        {
            if (!_repository.IsExist(id))
            {
                return NotFound();
            }

            return await _repository.GetByIdAsync(id);
        }

        // PUT: api/v1/ProductSizes/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductSize(int id, [FromBody] ProductSizeDto productSizeDto)
        {
            if (id != productSizeDto.Id)
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

            await _repository.UpdateAsync(productSizeDto);

            return NoContent();
        }

        // POST: api/v1/ProductSizes
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductSizeDto>> PostProductSize([FromBody] PostProductSizeDto postProductSizeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productSizeDto = await _repository.CreateAsync(postProductSizeDto);

            return CreatedAtAction(nameof(GetProductSize), new { id = productSizeDto.Id }, productSizeDto);
        }

        // DELETE: api/v1/ProductSizes/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductSize(int id)
        {
            if (!_repository.IsExist(id))
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);

            return NoContent();
        }
    }
}
