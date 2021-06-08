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
    [Route("api/v1/Colours")]
    [ApiController]
    public class ColoursController : ControllerBase
    {
        private readonly DataContext _context;

        public ColoursController(DataContext context)
        {
            _context = context;
        }

        // GET: api/v1/Colours
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Colour>>> GetColours([FromQuery] int productId)
        {
            if (productId != 0)
                return await _context.Colours.Where(i => i.ProductId == productId).ToListAsync();
            return await _context.Colours.ToListAsync();
        }

        // GET: api/v1/Colours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Colour>> GetColour(int id)
        {
            var colour = await _context.Colours.FindAsync(id);

            if (colour == null)
            {
                return NotFound();
            }

            return colour;
        }

        // PUT: api/v1/Colours/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColour(int id, [FromBody] Colour colour)
        {
            if (id != colour.Id)
            {
                return BadRequest();
            }

            _context.Entry(colour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColourExists(id))
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

        // POST: api/v1/Colours
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Colour>> PostColour([FromBody] Colour colour)
        {
            _context.Colours.Add(colour);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetColour), new { id = colour.Id }, colour);
        }

        // DELETE: api/v1/Colours/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Colour>> DeleteColour(int id)
        {
            var colour = await _context.Colours.FindAsync(id);
            if (colour == null)
            {
                return NotFound();
            }

            _context.Colours.Remove(colour);
            await _context.SaveChangesAsync();

            return colour;
        }

        private bool ColourExists(int id)
        {
            return _context.Colours.Any(e => e.Id == id);
        }
    }
}
