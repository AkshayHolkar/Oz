using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Dtos;
using Oz.Extensions;
using Oz.Repositories;
using System.Collections.Generic;
using System.Linq;
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

        // GET: api/v1/OrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetailDto>>> GetOrderDetails([FromQuery] int orderId)
        {
            if (orderId != 0)
            {
                return await _repository.GetAllAsync(orderId);
            }

            return NotFound();
        }

        // GET: api/v1/OrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailDto>> GetOrderDetail(int id)
        {
            if (!_repository.IsExist(id))
            {
                return NotFound();
            }

            return await _repository.GetByIdAsync(id);
        }

        // PUT: api/v1/OrderDetails/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
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
        public async Task<ActionResult<OrderDetailDto>> PostOrderDetail([FromBody] PostOrderDetailDto postOrderDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ErrorCount);
            }

            var orderDetailDto = await _repository.CreateAsync(postOrderDetailDto);

            return CreatedAtAction(nameof(GetOrderDetail), new { id = orderDetailDto.Id }, orderDetailDto);
        }

        // DELETE: api/v1/OrderDetails/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
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
