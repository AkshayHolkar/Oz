using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using Oz.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.OrderByDescending(j => j.Id).ToListAsync();
        }

        public async Task<List<Order>> GetAllByCustomerAsync(string customerId)
        {
            return await _context.Orders.Where(i => i.CustomerId == customerId).OrderByDescending(j => j.Id).ToListAsync();
        }

        public async Task<List<Order>> GetAllForCustomerAsync(string userId)
        {
            return await _context.Orders.Where(i => i.CustomerId == userId).OrderByDescending(j => j.Id).ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            var order = await _context.Orders.Include(i => i.OrderDetails).FirstOrDefaultAsync(i => i.Id == id);
            return order;
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Order> CreateAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public bool IsExist(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
