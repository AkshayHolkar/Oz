using Oz.Domain;
using Oz.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return new SingleOrderDto
            {
                Id = order.Id,
                DateCreation = order.DateCreation,
                CustomerId = order.CustomerId,
                OrderStatus = order.OrderStatus,
                OrderDetails = order.OrderDetails
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
                DateCreation = postOrderDto.DateCreation
            };
        }
    }
}
