using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Dtos;
using Oz.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Controllers.V1
{
    [Route("api/v1/ProductSizes")]
    [ApiController]
    public class ProductSizesController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductSizesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/v1/ProductSizes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductSizeDto>>> GetProductSizes([FromQuery] int productId)
        {
            if (productId != 0)
                return await _context.ProductSizes.Where(i => i.ProductId == productId).Select(product => product.AsDto()).ToListAsync();
            return await _context.ProductSizes.Select(product => product.AsDto()).ToListAsync();
        }

        // GET: api/v1/ProductSizes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductSizeDto>> GetProductSize(int id)
        {
            var productSize = await _context.ProductSizes.FindAsync(id);

            if (productSize == null)
            {
                return NotFound();
            }

            return productSize.AsDto();
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

            _context.Entry(productSizeDto.AsProductSizeFromProductSizeDto()).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductSizeExists(id))
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

        // POST: api/v1/ProductSizes
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductSizeDto>> PostProductSize([FromBody] PostProductSizeDto postProductSizeDto)
        {
            var productSize = postProductSizeDto.AsProductSizeFromPostProductSizeDto();
            _context.ProductSizes.Add(productSize);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductSize), new { id = productSize.Id }, productSize.AsDto());
        }

        // DELETE: api/v1/ProductSizes/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductSize(int id)
        {
            var productSize = await _context.ProductSizes.FindAsync(id);
            if (productSize == null)
            {
                return NotFound();
            }

            _context.ProductSizes.Remove(productSize);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductSizeExists(int id)
        {
            return _context.ProductSizes.Any(e => e.Id == id);
        }
    }
}
