using Oz.Domain;
using Oz.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Extensions
{
    public static class ImageDtosExtension
    {
        public static ImageDto AsDto(this Image image)
        {
            return new ImageDto
            {
                Id = image.Id,
                Name = image.Name,
                Main = image.Main,
                ProductId = image.ProductId,
                ImageScr = image.ImageScr
            };
        }

        public static Image AsImageFromPutImageDto(this PutImageDto image)
        {
            return new Image
            {
                Id = image.Id,
                Name = image.Name,
                Main = image.Main,
                ProductId = image.ProductId,
            };
        }
    }
}
