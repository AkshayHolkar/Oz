using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oz.Dtos;
using Oz.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Oz.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/OrderDetails")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailRepository _repository;

        public OrderDetailsController(IOrderDetailRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// If OrderId Provided Returns All OrderDetails For That OrderId Else Return BadRequest
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        // GET: api/v1/OrderDetails
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderDetailDto>>> GetOrderDetails([FromQuery] int orderId = 0)
        {
            if (orderId != 0)
            {
                return await _repository.GetAllAsync(orderId);
            }

            return BadRequest();
        }

        // GET: api/v1/OrderDetails/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDetailDto>> GetOrderDetail(int id)
        {
            if (!_repository.IsExist(id))
            {
                return NotFound();
            }

            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orderDetailDto"></param>
        /// <returns></returns>
        // PUT: api/v1/OrderDetails/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutOrderDetail(int id, [FromBody] OrderDetailDto orderDetailDto)
        {
            if (id != orderDetailDto.Id)
            {
                return BadRequest();
            }

            if (!_repository.IsExist(orderDetailDto.Id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ErrorCount);
            }

            await _repository.UpdateAsync(orderDetailDto);

            return NoContent();
        }

        // POST: api/v1/OrderDetails
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDetailDto>> PostOrderDetail([FromBody] PostOrderDetailDto postOrderDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ErrorCount);
            }

            var orderDetailDto = await _repository.CreateAsync(postOrderDetailDto);

            return CreatedAtAction(nameof(GetOrderDetail), new { id = orderDetailDto.Id }, orderDetailDto);
        }

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/v1/OrderDetails/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrderDetail(int id)
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
