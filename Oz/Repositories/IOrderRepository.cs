using Microsoft.AspNetCore.Mvc;
using Oz.Domain;
using Oz.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public interface IOrderRepository
    {
        Task<List<OrderDto>> GetAllAsync();
        Task<List<OrderDto>> GetAllByCustomerAsync(string customerId);
        Task<List<OrderDto>> GetAllForCustomerAsync(string userId);
        Task<SingleOrderDto> GetByIdAsync(int id);
        Task<OrderDto> CreateAsync(Order order);
        Task UpdateAsync(OrderDto orderDto);
        Task DeleteAsync(int id);
        bool IsExist(int id);
    }
}
