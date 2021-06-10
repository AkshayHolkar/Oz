using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Oz.Extensions;

namespace Oz.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/v1/Carts")]
    public class CartsController : ControllerBase
    {
        private readonly DataContext _context;

        public CartsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/v1/Carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            return await _context.Carts.Where(i => i.CustomerId == HttpContext.GetUserId()).ToListAsync();

        }

        // GET: api/v1/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            var userOwnCart = UserOwnsCart(cart, HttpContext.GetUserId());

            if (!userOwnCart)
            {
                return BadRequest(new { error = "You do not own this cart" });
            }

            return cart;
        }

        // PUT: api/v1/Carts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, [FromBody] Cart cart)
        {
            if (id != cart.Id)
            {
                return BadRequest();
            }

            var userOwnCart = UserOwnsCart(cart, HttpContext.GetUserId());

            if (!userOwnCart)
            {
                return BadRequest(new { error = "You do not own this cart" });
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
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

        // POST: api/v1/Carts
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart([FromBody] Cart cart)
        {
            cart.CustomerId = HttpContext.GetUserId();

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCart), new { id = cart.Id }, cart);
        }

        // DELETE: api/v1/Carts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cart>> DeleteCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            var userOwnCart = UserOwnsCart(cart, HttpContext.GetUserId());

            if (!userOwnCart)
            {
                return BadRequest(new { error = "You do not own this cart" });
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return cart;
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }

        private bool UserOwnsCart(Cart cart, string userId)
        {
            if (cart.CustomerId != userId)
            {
                return false;
            }

            return true;
        }
    }
}
