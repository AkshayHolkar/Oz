using Microsoft.AspNetCore.Mvc;
using Oz.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public interface IOrderDetailRepository
    {
        Task<ActionResult<IEnumerable<OrderDetailDto>>> GetAllAsync(int orderId);
        Task<ActionResult<OrderDetailDto>> GetByIdAsync(int id);
        Task<OrderDetailDto> CreateAsync(PostOrderDetailDto postOrderDetailDto);
        Task<ActionResult<bool>> UpdateAsync(OrderDetailDto orderDetailDto);
        Task<ActionResult<bool>> DeleteAsync(int id);
        bool IsExist(int id);
    }
}
