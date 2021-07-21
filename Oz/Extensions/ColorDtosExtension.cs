using Oz.Domain;
using Oz.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Extensions
{
    public static class ColorDtosExtension
    {
        public static ColourDto AsDto(this Colour colour)
        {
            return new ColourDto
            {
                Id = colour.Id,
                ProductId = colour.ProductId,
                Color = colour.Color,
                ProductQuantity = colour.ProductQuantity
            };
        }

        public static Colour AsColourFromColourDto(this ColourDto colourDto)
        {
            return new Colour
            {
                Id = colourDto.Id,
                ProductId = colourDto.ProductId,
                Color = colourDto.Color,
                ProductQuantity = colourDto.ProductQuantity
            };
        }

        public static Colour AsCartFromPostColourDto(this PostColourDto postColourDto)
        {
            return new Colour()
            {
                ProductId = postColourDto.ProductId,
                Color = postColourDto.Color,
                ProductQuantity = postColourDto.ProductQuantity
            };
        }
    }
}
