using Oz.Domain;
using Oz.Dtos;

namespace Oz.Extensions
{
    public static class OrderDetailDtosExtension
    {
        public static OrderDetailDto AsDto(this OrderDetail orderDetail)
        {
            return new OrderDetailDto
            {
                Id = orderDetail.Id,
                OrderId = orderDetail.OrderId,
                ProductId = orderDetail.ProductId,
                ProductName = orderDetail.ProductName,
                Color = orderDetail.Color,
                Size = orderDetail.Size,
                TotalPrice = orderDetail.TotalPrice,
                Quantity = orderDetail.Quantity
            };
        }

        public static OrderDetail AsOrderFromOrderDetailDto(this OrderDetailDto orderDetail)
        {
            return new OrderDetail()
            {
                Id = orderDetail.Id,
                OrderId = orderDetail.OrderId,
                ProductId = orderDetail.ProductId,
                ProductName = orderDetail.ProductName,
                Color = orderDetail.Color,
                Size = orderDetail.Size,
                TotalPrice = orderDetail.TotalPrice,
                Quantity = orderDetail.Quantity
            };
        }

        public static OrderDetail AsOrderFromPostOrderDetailDto(this PostOrderDetailDto orderDetail)
        {
            return new OrderDetail()
            {
                OrderId = orderDetail.OrderId,
                ProductId = orderDetail.ProductId,
                ProductName = orderDetail.ProductName,
                Color = orderDetail.Color,
                Size = orderDetail.Size,
                TotalPrice = orderDetail.TotalPrice,
                Quantity = orderDetail.Quantity
            };
        }
    }
}
