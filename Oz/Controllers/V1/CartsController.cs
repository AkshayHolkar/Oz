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
using Oz.Services;
using Oz.Dtos;

namespace Oz.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/v1/Carts")]
    public class CartsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ISharedService _sharedService;

        public CartsController(DataContext context, ISharedService sharedService)
        {
            _context = context;
            _sharedService = sharedService;
        }

        // GET: api/v1/Carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartDto>>> GetCarts()
        {
            return await _context.Carts.Where(i => i.CustomerId == HttpContext.GetUserId()).Select(cart => cart.AsDto()).ToListAsync();

        }

        // GET: api/v1/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartDto>> GetCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            if (!_sharedService.UserOwnsDomain(cart.CustomerId, HttpContext.GetUserId()))
            {
                return BadRequest(new { error = "You do not own this cart" });
            }

            return cart.AsDto();
        }

        // PUT: api/v1/Carts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, [FromBody] CartDto cartDto)
        {
            if (id != cartDto.Id)
            {
                return BadRequest();
            }

            if (!_sharedService.UserOwnsDomain(cartDto.CustomerId, HttpContext.GetUserId()))
            {
                return BadRequest(new { error = "You do not own this cart" });
            }

            _context.Entry(cartDto.AsCartFromCartDto()).State = EntityState.Modified;

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
        public async Task<ActionResult<CartDto>> PostCart([FromBody] CreateCartDto createCartDto)
        {
            var cart = createCartDto.AsCartFromCreateCartDto();
            cart.CustomerId = HttpContext.GetUserId();

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCart), new { id = cart.Id }, cart.AsDto());
        }

        // DELETE: api/v1/Carts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            if (!_sharedService.UserOwnsDomain(cart.CustomerId, HttpContext.GetUserId()))
            {
                return BadRequest(new { error = "You do not own this cart" });
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
