using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Controllers.V1
{
    [Route("api/v1/OrderStatuses")]
    [ApiController]
    public class OrderStatusesController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderStatusesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/v1/OrderStatuses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderStatus>>> GetOrderStatuses()
        {
            return await _context.OrderStatuses.ToListAsync();
        }

        // GET: api/v1/OrderStatuses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderStatus>> GetOrderStatus(int id)
        {
            var orderStatus = await _context.OrderStatuses.FindAsync(id);

            if (orderStatus == null)
            {
                return NotFound();
            }

            return orderStatus;
        }

        // PUT: api/v1/OrderStatuses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderStatus(int id, [FromBody] OrderStatus orderStatus)
        {
            if (id != orderStatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(orderStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderStatusExists(id))
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

        // POST: api/v1/OrderStatuses
        [HttpPost]
        public async Task<ActionResult<OrderStatus>> PostOrderStatus([FromBody] OrderStatus orderStatus)
        {
            _context.OrderStatuses.Add(orderStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderStatus), new { id = orderStatus.Id }, orderStatus);
        }

        // DELETE: api/v1/OrderStatuses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderStatus>> DeleteOrderStatus(int id)
        {
            var orderStatus = await _context.OrderStatuses.FindAsync(id);
            if (orderStatus == null)
            {
                return NotFound();
            }

            _context.OrderStatuses.Remove(orderStatus);
            await _context.SaveChangesAsync();

            return orderStatus;
        }

        private bool OrderStatusExists(int id)
        {
            return _context.OrderStatuses.Any(e => e.Id == id);
        }
    }
}
