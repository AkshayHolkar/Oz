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
        public async Task<ActionResult<IEnumerable<Colour>>> GetColours()
        {
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
        [HttpPost]
        public async Task<ActionResult<Colour>> PostColour([FromBody] Colour colour)
        {
            _context.Colours.Add(colour);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetColour), new { id = colour.Id }, colour);
        }

        // DELETE: api/v1/Colours/5
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
