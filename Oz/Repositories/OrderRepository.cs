using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using Oz.Dtos;
using Oz.Extensions;
using System;
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

        public async Task<List<OrderDto>> GetAllAsync()
        {
            return await _context.Orders.OrderByDescending(j => j.Id).Select(order => order.AsDto()).ToListAsync();
        }

        public async Task<List<OrderDto>> GetAllByCustomerAsync(string customerId)
        {
            return await _context.Orders.Where(i => i.CustomerId == customerId).OrderByDescending(j => j.Id).Select(order => order.AsDto()).ToListAsync();
        }

        public async Task<List<OrderDto>> GetAllForCustomerAsync(string userId)
        {
            return await _context.Orders.Where(i => i.CustomerId == userId).OrderByDescending(j => j.Id).Select(order => order.AsDto()).ToListAsync();
        }

        public async Task<SingleOrderDto> GetByIdAsync(int id)
        {
            var order = await _context.Orders.Include(i => i.OrderDetails).FirstOrDefaultAsync(i => i.Id == id);
            return order.AsSingleOrderDto();
        }

        public async Task UpdateAsync(OrderDto orderDto)
        {
            _context.Entry(orderDto.AsOrderFromOrderDto()).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<OrderDto> CreateAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.AsDto();
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
