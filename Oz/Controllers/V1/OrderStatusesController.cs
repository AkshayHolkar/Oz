using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using Oz.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/OrderStatuses")]
    [ApiController]
    public class OrderStatusesController : ControllerBase
    {
        private readonly IDomainsRepository<OrderStatus> _repository;

        public OrderStatusesController(IDomainsRepository<OrderStatus> repository)
        {
            _repository = repository;
        }

        // GET: api/v1/OrderStatuses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderStatus>>> GetOrderStatuses()
        {
            return await _repository.GetAllAsync();
        }

        // GET: api/v1/OrderStatuses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderStatus>> GetOrderStatus(int id)
        {
            var orderStatus = await _repository.GetByIdAsync(id);

            if (orderStatus == null)
            {
                return NotFound();
            }

            return orderStatus;
        }

        // PUT: api/v1/OrderStatuses/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
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

        // POST: api/v1/OrderStatuses
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<OrderStatus>> PostOrderStatus([FromBody] OrderStatus orderStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ErrorCount);
            }

            orderStatus = await _repository.CreateAsync(orderStatus);

            return CreatedAtAction(nameof(GetOrderStatus), new { id = orderStatus.Id }, orderStatus);
        }

        // DELETE: api/v1/OrderStatuses/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderStatus>> DeleteOrderStatus(int id)
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
