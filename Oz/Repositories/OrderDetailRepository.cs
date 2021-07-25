using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Dtos;
using Oz.Extensions;
using System;
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

        public async Task<List<OrderDetailDto>> GetAllAsync(int orderId)
        {
            return await _context.OrderDetails.Where(i => i.OrderId == orderId).Select(orderDetail => orderDetail.AsDto()).ToListAsync();
        }

        public async Task<OrderDetailDto> GetByIdAsync(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            return orderDetail.AsDto();
        }

        public async Task<OrderDetailDto> CreateAsync(PostOrderDetailDto postOrderDetailDto)
        {
            var orderDetail = postOrderDetailDto.AsOrderFromPostOrderDetailDto();
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();
            return orderDetail.AsDto();
        }

        public async Task UpdateAsync(OrderDetailDto orderDetailDto)
        {
            _context.Entry(orderDetailDto.AsOrderFromOrderDetailDto()).State = EntityState.Modified;
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
