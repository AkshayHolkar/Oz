using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using Oz.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public class OrderStatusRepository : IOrderStatusRepository
    {


        private readonly DataContext _context;

        public OrderStatusRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<OrderStatus>> GetAllAsync()
        {
            return await _context.OrderStatuses.ToListAsync();
        }

        public async Task<OrderStatus> GetByIdAsync(int id)
        {
            return await _context.OrderStatuses.FindAsync(id);
        }

        public async Task<OrderStatus> CreateAsync(OrderStatus entity)
        {
            _context.OrderStatuses.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(OrderStatus entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var orderStatus = await _context.OrderStatuses.FindAsync(id);
            _context.OrderStatuses.Remove(orderStatus);
            await _context.SaveChangesAsync();
        }

        public bool IsExist(int id)
        {
            return _context.OrderStatuses.Any(e => e.Id == id);
        }
    }
}
