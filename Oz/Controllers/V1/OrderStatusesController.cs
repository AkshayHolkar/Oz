using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oz.Domain;
using Oz.Dtos;
using Oz.Repositories.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Oz.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/OrderStatuses")]
    [ApiController]
    public class OrderStatusesController : ControllerBase
    {
        private readonly IOrderStatusRepository _repository;

        public OrderStatusesController(IOrderStatusRepository repository)
        {
            _repository = repository;
        }

        // GET: api/v1/OrderStatuses
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<OrderStatus>>> GetOrderStatuses()
        {
            return Ok(await _repository.GetAllAsync());
        }

        // GET: api/v1/OrderStatuses/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderStatus>> GetOrderStatus(int id)
        {
            var orderStatus = await _repository.GetByIdAsync(id);

            if (orderStatus == null)
            {
                return NotFound();
            }

            return Ok(orderStatus);
        }

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        // PUT: api/v1/OrderStatuses/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutOrderStatus(int id, [FromBody] OrderStatus orderStatus)
        {
            if (id != orderStatus.Id)
            {
                return BadRequest();
            }

            if (!_repository.IsExist(id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ErrorCount);
            }

            await _repository.UpdateAsync(orderStatus);

            return NoContent();
        }

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="postOrderStatusDto"></param>
        /// <returns></returns>
        // POST: api/v1/OrderStatuses
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderStatus>> PostOrderStatus([FromBody] PostOrderStatusDto postOrderStatusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ErrorCount);
            }

            var orderStatus = new OrderStatus { Name = postOrderStatusDto.Name };
            orderStatus = await _repository.CreateAsync(orderStatus);

            return CreatedAtAction(nameof(GetOrderStatus), new { id = orderStatus.Id }, orderStatus);
        }

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/v1/OrderStatuses/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrderStatus(int id)
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
