using Oz.Domain;
using Oz.Dtos;

namespace Oz.Extensions
{
    public static class ProductDtosExtension
    {
        public static ProductDto AsDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description,
                Status = product.Status,
                CategoryId = product.CategoryId,
                SizeNotApplicable = product.SizeNotApplicable,
                ColorNotApplicable = product.ColorNotApplicable
            };
        }

        public static Product AsProductFromProductDto(this ProductDto product)
        {
            return new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description,
                Status = product.Status,
                CategoryId = product.CategoryId,
                SizeNotApplicable = product.SizeNotApplicable,
                ColorNotApplicable = product.ColorNotApplicable
            };
        }

        public static Product AsProductFromPostProductDto(this PostProductDto product)
        {
            return new Product
            {
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description,
                Status = product.Status,
                CategoryId = product.CategoryId,
                SizeNotApplicable = product.SizeNotApplicable,
                ColorNotApplicable = product.ColorNotApplicable
            };
        }
    }
}
