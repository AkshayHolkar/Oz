using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using Oz.Dtos;
using Oz.Extensions;
using Oz.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Controllers.V1
{
    [Route("api/v1/Products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        // GET: api/v1/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            return await _repository.GetAllAsync();
        }

        // GET: api/v1/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/v1/Products/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] ProductDto productDto)
        {
            if (id != productDto.Id)
            {
                return BadRequest();
            }

            if (!_repository.IsExist(productDto.Id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ErrorCount);
            }

            await _repository.UpdateAsync(productDto);

            return NoContent();
        }

        // POST: api/v1/Products
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProduct([FromBody] PostProductDto postProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ErrorCount);
            }

            var product = await _repository.CreateAsync(postProductDto);

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // DELETE: api/v1/Products/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
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
