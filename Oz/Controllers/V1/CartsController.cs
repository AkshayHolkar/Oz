using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Oz.Extensions;
using Oz.Services;
using Oz.Dtos;
using Oz.Repositories;

namespace Oz.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/v1/Carts")]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository _repository;
        private readonly ISharedService _sharedService;

        public CartsController(ICartRepository repository, ISharedService sharedService)
        {
            _repository = repository;
            _sharedService = sharedService;
        }

        // GET: api/v1/Carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartDto>>> GetCarts()
        {
            var userId = HttpContext.GetUserId();
            return await _repository.GetAllAsync(userId);
        }

        // GET: api/v1/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartDto>> GetCart(int id)
        {
            if (!_repository.IsExist(id))
            {
                return NotFound();
            }
            var cart = await _repository.GetByIdAsync(id);

            if (!_sharedService.UserOwnsDomain(cart.Value.CustomerId, HttpContext.GetUserId()))
            {
                return BadRequest(new { error = "You do not own this cart" });
            }

            return cart;
        }

        // PUT: api/v1/Carts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, [FromBody] CartDto cartDto)
        {

            if (id != cartDto.Id)
            {
                return BadRequest();
            }

            if (!_repository.IsExist(cartDto.Id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ErrorCount);
            }

            await _repository.UpdateAsync(cartDto);

            return NoContent();
        }

        // POST: api/v1/Carts
        [HttpPost]
        public async Task<ActionResult<CartDto>> PostCart([FromBody] CreateCartDto createCartDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cart = createCartDto.AsCartFromCreateCartDto();
            cart.CustomerId = HttpContext.GetUserId();

            var cartDto = await _repository.CreateAsync(cart);

            return CreatedAtAction(nameof(GetCart), new { id = cart.Id }, cartDto);
        }

        // DELETE: api/v1/Carts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _repository.GetByIdAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            if (!_sharedService.UserOwnsDomain(cart.Value.CustomerId, HttpContext.GetUserId()))
            {
                return BadRequest(new { error = "You do not own this cart" });
            }

            await _repository.DeleteAsync(id);

            return NoContent();
        }
    }
}
