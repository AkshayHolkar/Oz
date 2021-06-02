using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Controllers.V1
{
    [Route("api/v1/Sizes")]
    [ApiController]
    public class SizesController : ControllerBase
    {
        private readonly DataContext _context;

        public SizesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/v1/Sizes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Size>>> GetSizes()
        {
            return await _context.Sizes.ToListAsync();
        }

        // GET: api/v1/Sizes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Size>> GetSize(int id)
        {
            var size = await _context.Sizes.FindAsync(id);

            if (size == null)
            {
                return NotFound();
            }

            return size;
        }

        // PUT: api/v1/Sizes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSize(int id, [FromBody] Size size)
        {
            if (id != size.Id)
            {
                return BadRequest();
            }

            _context.Entry(size).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SizeExists(id))
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

        // POST: api/v1/Sizes
        [HttpPost]
        public async Task<ActionResult<Size>> PostSize([FromBody] Size size)
        {
            _context.Sizes.Add(size);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSize), new { id = size.Id }, size);
        }

        // DELETE: api/v1/Sizes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Size>> DeleteSize(int id)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }

            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();

            return size;
        }

        private bool SizeExists(int id)
        {
            return _context.Sizes.Any(e => e.Id == id);
        }
    }
}

