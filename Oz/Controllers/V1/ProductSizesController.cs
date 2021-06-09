using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
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
        public async Task<ActionResult<IEnumerable<ProductSize>>> GetProductSizes([FromQuery] int productId)
        {
            if (productId != 0)
                return await _context.ProductSizes.Where(i => i.ProductId == productId).ToListAsync();
            return await _context.ProductSizes.ToListAsync();
        }

        // GET: api/v1/ProductSizes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductSize>> GetProductSize(int id)
        {
            var productSize = await _context.ProductSizes.FindAsync(id);

            if (productSize == null)
            {
                return NotFound();
            }

            return productSize;
        }

        // PUT: api/v1/ProductSizes/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductSize(int id, [FromBody] ProductSize productSize)
        {
            if (id != productSize.Id)
            {
                return BadRequest();
            }

            _context.Entry(productSize).State = EntityState.Modified;

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
        public async Task<ActionResult<ProductSize>> PostProductSize([FromBody] ProductSize productSize)
        {
            _context.ProductSizes.Add(productSize);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductSize), new { id = productSize.Id }, productSize);
        }

        // DELETE: api/v1/ProductSizes/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductSize>> DeleteProductSize(int id)
        {
            var productSize = await _context.ProductSizes.FindAsync(id);
            if (productSize == null)
            {
                return NotFound();
            }

            _context.ProductSizes.Remove(productSize);
            await _context.SaveChangesAsync();

            return productSize;
        }

        private bool ProductSizeExists(int id)
        {
            return _context.ProductSizes.Any(e => e.Id == id);
        }
    }
}
