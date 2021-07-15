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
using Microsoft.AspNetCore.Identity;

namespace Oz.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/v1/Orders")]
    public class OrdersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(DataContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/v1/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var userId = HttpContext.GetUserId();
            if (await IsAdmin(userId))
                return await _context.Orders.ToListAsync();
            return await _context.Orders.Where(i => i.CustomerId == userId).ToListAsync();

        }

        // GET: api/v1/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                 .Include(i => i.OrderDetails)
                        .FirstOrDefaultAsync(i => i.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var userOwnOrder = UserOwnsOrder(order, HttpContext.GetUserId());

            if (!userOwnOrder)
            {
                return BadRequest(new { error = "You do not own this order" });
            }

            return order;
        }

        // PUT: api/v1/Orders/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, [FromBody] Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/v1/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder([FromBody] Order order)
        {
            var userId = HttpContext.GetUserId();
            if (!await IsAdmin(userId))
                order.CustomerId = userId;
            order.OrderStatus = "In Progress";

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        // DELETE: api/v1/Orders/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var userOwnOrder = UserOwnsOrder(order, HttpContext.GetUserId());

            if (!userOwnOrder)
            {
                return BadRequest(new { error = "You do not own this order" });
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        private bool UserOwnsOrder(Order order, string userId)
        {
            if (order.CustomerId != userId)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> IsAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            foreach (string role in roles)
            {
                if (role == "Admin")
                    return true;
            }

            return false;
        }
    }
}
