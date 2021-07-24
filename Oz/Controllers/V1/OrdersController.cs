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
using Oz.Repositories;

namespace Oz.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/v1/Orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repository;
        private readonly IIdentityService _identityService;
        private readonly ISharedService _sharedService;

        public OrdersController(IOrderRepository repository, IIdentityService identityService, ISharedService sharedService)
        {
            _repository = repository;
            _identityService = identityService;
            _sharedService = sharedService;
        }

        // GET: api/v1/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders([FromQuery] string customerId)
        {
            var userId = HttpContext.GetUserId();
            if (await _identityService.IsAdminAsync(userId) && string.IsNullOrEmpty(customerId))
                return await _repository.GetAllAsync();

            if (await _identityService.IsAdminAsync(userId) && !string.IsNullOrEmpty(customerId))
                return await _repository.GetAllByCustomerAsync(customerId);

            return await _repository.GetAllForCustomerAsync(userId);
        }

        // GET: api/v1/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SingleOrderDto>> GetOrder(int id)
        {
            if (!_repository.IsExist(id))
            {
                return NotFound();
            }

            var singleOrderDto = await _repository.GetByIdAsync(id);

            if (!_sharedService.UserOwnsDomain(singleOrderDto.Value.CustomerId, HttpContext.GetUserId()))
            {
                return BadRequest(new { error = "You do not own this order" });
            }

            return singleOrderDto;
        }

        // PUT: api/v1/Orders/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, [FromBody] OrderDto orderDto)
        {
            if (id != orderDto.Id)
            {
                return BadRequest();
            }

            if (!_repository.IsExist(orderDto.Id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ErrorCount);
            }

            await _repository.UpdateAsync(orderDto);

            return NoContent();
        }

        // POST: api/v1/Orders
        [HttpPost]
        public async Task<ActionResult<OrderDto>> PostOrder([FromBody] PostOrderDto postOrderDto)
        {
            var order = postOrderDto.AsOrderFromPostOrderDto();
            var userId = HttpContext.GetUserId();
            if (!await _identityService.IsAdminAsync(userId))
                order.CustomerId = userId;
            order.OrderStatus = "In Progress";

            var orderDto = await _repository.CreateAsync(order);

            return CreatedAtAction(nameof(GetOrder), new { id = orderDto.Id }, orderDto);
        }

        // DELETE: api/v1/Orders/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
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
