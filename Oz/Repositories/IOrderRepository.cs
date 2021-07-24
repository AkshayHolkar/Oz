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
        Task<ActionResult<IEnumerable<OrderDto>>> GetAllAsync();
        Task<ActionResult<IEnumerable<OrderDto>>> GetAllByCustomerAsync(string customerId);
        Task<ActionResult<IEnumerable<OrderDto>>> GetAllForCustomerAsync(string userId);
        Task<ActionResult<SingleOrderDto>> GetByIdAsync(int id);
        Task<OrderDto> CreateAsync(Order order);
        Task<ActionResult<bool>> UpdateAsync(OrderDto orderDto);
        Task<ActionResult<bool>> DeleteAsync(int id);
        bool IsExist(int id);
    }
}
