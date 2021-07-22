using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public class OrderStatusRepository : IDomainsRepository<OrderStatus>
    {


        private readonly DataContext _context;

        public OrderStatusRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<OrderStatus>>> GetAllAsync()
        {
            return await _context.OrderStatuses.ToListAsync();
        }

        public async Task<ActionResult<OrderStatus>> GetByIdAsync(int id)
        {
            return await _context.OrderStatuses.FindAsync(id);
        }

        public async Task<OrderStatus> CreateAsync(OrderStatus entity)
        {
            _context.OrderStatuses.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<ActionResult<bool>> UpdateAsync(OrderStatus entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ActionResult<bool>> DeleteAsync(int id)
        {
            var orderStatus = await _context.OrderStatuses.FindAsync(id);
            _context.OrderStatuses.Remove(orderStatus);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool isExist(int id)
        {
            return _context.OrderStatuses.Any(e => e.Id == id);
        }
    }
}
