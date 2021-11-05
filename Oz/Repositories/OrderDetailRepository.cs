using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using Oz.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly DataContext _context;

        public OrderDetailRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDetail>> GetAllAsync(int orderId)
        {
            return await _context.OrderDetails.Where(i => i.OrderId == orderId).ToListAsync();
        }

        public async Task<OrderDetail> GetByIdAsync(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            return orderDetail;
        }

        public async Task<OrderDetail> CreateAsync(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();
            return orderDetail;
        }

        public async Task UpdateAsync(OrderDetail orderDetail)
        {
            _context.Entry(orderDetail).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
        }

        public bool IsExist(int id)
        {
            return _context.OrderDetails.Any(e => e.Id == id);
        }
    }
}
