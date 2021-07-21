using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Dtos;
using Oz.Extensions;
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
        private readonly DataContext _context;

        public OrderDetailsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/v1/OrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetailDto>>> GetOrderDetails([FromQuery] int orderId)
        {
            if (orderId != 0)
            {
                return await _context.OrderDetails.Where(i => i.OrderId == orderId).Select(orderDetail => orderDetail.AsDto()).ToListAsync();
            }

            return NotFound();
        }

        // GET: api/v1/OrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailDto>> GetOrderDetail(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return orderDetail.AsDto();
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

            _context.Entry(orderDetailDto.AsOrderFromOrderDetailDto()).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(id))
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

        // POST: api/v1/OrderDetails
        [HttpPost]
        public async Task<ActionResult<OrderDetailDto>> PostOrderDetail([FromBody] PostOrderDetailDto postOrderDetailDto)
        {
            var orderDetail = postOrderDetailDto.AsOrderFromPostOrderDetailDto();
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderDetail), new { id = orderDetail.Id }, orderDetail.AsDto());
        }

        // DELETE: api/v1/OrderDetails/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails.Any(e => e.Id == id);
        }
    }
}
