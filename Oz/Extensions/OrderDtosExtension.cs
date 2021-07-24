using Oz.Domain;
using Oz.Dtos;
using System.Collections.Generic;

namespace Oz.Extensions
{
    public static class OrderDtosExtension
    {
        public static OrderDto AsDto(this Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                DateCreation = order.DateCreation,
                CustomerId = order.CustomerId,
                OrderStatus = order.OrderStatus
            };
        }

        public static SingleOrderDto AsSingleOrderDto(this Order order)
        {
            var orderDetailsAsDto = GetOrderDetailsAsDto(order.OrderDetails);
            return new SingleOrderDto
            {
                Id = order.Id,
                DateCreation = order.DateCreation,
                CustomerId = order.CustomerId,
                OrderStatus = order.OrderStatus,
                OrderDetails = orderDetailsAsDto
            };
        }

        public static Order AsOrderFromOrderDto(this OrderDto orderDto)
        {
            return new Order()
            {
                Id = orderDto.Id,
                DateCreation = orderDto.DateCreation,
                CustomerId = orderDto.CustomerId,
                OrderStatus = orderDto.OrderStatus
            };
        }

        public static Order AsOrderFromPostOrderDto(this PostOrderDto postOrderDto)
        {
            return new Order()
            {
                CustomerId = postOrderDto.CustomerId,
                DateCreation = postOrderDto.DateCreation
            };
        }

        private static ICollection<OrderDetailDto> GetOrderDetailsAsDto(ICollection<OrderDetail> orderDetails)
        {
            var orderDetailDtos = new List<OrderDetailDto>();

            foreach (var orderDetail in orderDetails)
            {
                orderDetailDtos.Add(orderDetail.AsDto());
            }

            return orderDetailDtos;
        }
    }
}
