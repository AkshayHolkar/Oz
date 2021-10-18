using Microsoft.AspNetCore.Mvc;
using Oz.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public interface IOrderDetailRepository
    {
        Task<List<OrderDetailDto>> GetAllAsync(int orderId);
        Task<OrderDetailDto> GetByIdAsync(int id);
        Task<OrderDetailDto> CreateAsync(PostOrderDetailDto postOrderDetailDto);
        Task UpdateAsync(OrderDetailDto orderDetailDto);
        Task DeleteAsync(int id);
        bool IsExist(int id);
    }
}
