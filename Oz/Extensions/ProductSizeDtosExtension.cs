using Oz.Domain;
using Oz.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Extensions
{
    public static class ProductSizeDtosExtension
    {
        public static ProductSizeDto AsDto(this ProductSize productSize)
        {
            return new ProductSizeDto
            {
                Id = productSize.Id,
                ProductId = productSize.ProductId,
                ItemSize = productSize.ItemSize
            };
        }

        public static ProductSize AsProductSizeFromProductSizeDto(this ProductSizeDto productSize)
        {
            return new ProductSize
            {
                Id = productSize.Id,
                ProductId = productSize.ProductId,
                ItemSize = productSize.ItemSize
            };
        }

        public static ProductSize AsProductSizeFromPostProductSizeDto(this PostProductSizeDto productSize)
        {
            return new ProductSize
            {
                ProductId = productSize.ProductId,
                ItemSize = productSize.ItemSize
            };
        }
    }
}
