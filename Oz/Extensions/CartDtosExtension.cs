using Oz.Domain;
using Oz.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Extensions
{
    public static class CartDtosExtension
    {
        public static CartDto AsDto(this Cart cart)
        {
            return new CartDto
            {
                Id = cart.Id,
                CustomerId = cart.CustomerId,
                ProductId = cart.ProductId,
                ProductName = cart.ProductName,
                ImageUrl = cart.ImageUrl,
                Color = cart.Color,
                Size = cart.Size,
                Quantity = cart.Quantity,
                MaxLimit = cart.MaxLimit,
                Price = cart.Price
            };
        }

        public static Cart AsCartFromCartDto(this CartDto cartDto)
        {
            return new Cart()
            {
                Id = cartDto.Id,
                CustomerId = cartDto.CustomerId,
                ProductId = cartDto.ProductId,
                ProductName = cartDto.ProductName,
                ImageUrl = cartDto.ImageUrl,
                Color = cartDto.Color,
                Size = cartDto.Size,
                Quantity = cartDto.Quantity,
                MaxLimit = cartDto.MaxLimit,
                Price = cartDto.Price
            };
        }

        public static Cart AsCartFromCreateCartDto(this CreateCartDto cartDto)
        {
            return new Cart()
            {
                ProductId = cartDto.ProductId,
                ProductName = cartDto.ProductName,
                ImageUrl = cartDto.ImageUrl,
                Color = cartDto.Color,
                Size = cartDto.Size,
                Quantity = cartDto.Quantity,
                MaxLimit = cartDto.MaxLimit,
                Price = cartDto.Price
            };
        }
    }
}
