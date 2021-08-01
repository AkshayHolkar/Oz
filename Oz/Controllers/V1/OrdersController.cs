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

        /// <summary>
        /// If Administrator Returns All Orders Else Only User Orders. If Administrator Provide CustomerId Returns All Orders Of That Customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        // GET: api/v1/Orders
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SingleOrderDto>> GetOrder(int id)
        {
            if (!_repository.IsExist(id))
            {
                return NotFound();
            }

            var singleOrderDto = await _repository.GetByIdAsync(id);

            if (!_sharedService.UserOwnsDomain(singleOrderDto.CustomerId, HttpContext.GetUserId()))
            {
                return BadRequest(new { error = "You do not own this order" });
            }

            return singleOrderDto;
        }

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        // PUT: api/v1/Orders/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// To Create Own Order User Does Not Need To Provide CustomerId. Only Administrator Can Create Order For Customer by Providing CustomerId
        /// </summary>
        /// <param name="postOrderDto"></param>
        /// <returns></returns>
        // POST: api/v1/Orders
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/v1/Orders/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
