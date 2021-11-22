using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Oz.Controllers.V1;
using Oz.Domain;
using Oz.Dtos;
using Oz.Extensions;
using Oz.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Oz.UnitTests
{
    public class ProductSizesControllerTests
    {
        private readonly IProductSizeRepository mockRepository = A.Fake<IProductSizeRepository>();

        [Fact]
        public async Task GetProductSizes_WithOutProductId_ReturnsAllProductSizes()
        {
            //Arrage
            var expectedProductSizes = (List<ProductSize>)A.CollectionOfDummy<ProductSize>(5);
            A.CallTo(() => mockRepository.GetAllAsync())
                .Returns(Task.FromResult(expectedProductSizes));
            var controller = new ProductSizesController(mockRepository);

            //Act
            var result = await controller.GetProductSizes();
            var okResult = result.Result as ObjectResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(expectedProductSizes.Select(productSize => productSize.AsDto()));
        }

        [Fact]
        public async Task GetProductSizes_WithProductId_ReturnsAllProductSizes()
        {
            //Arrage
            int productId = 2;
            var expectedProductSizes = (List<ProductSize>)A.CollectionOfDummy<ProductSize>(5);
            A.CallTo(() => mockRepository.GetAllProductSizesByProductIdAsync(productId))
                .Returns(Task.FromResult(expectedProductSizes));
            var controller = new ProductSizesController(mockRepository);

            //Act
            var result = await controller.GetProductSizes(productId);
            var okResult = result.Result as ObjectResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(expectedProductSizes.Select(productSize => productSize.AsDto()));
        }

        [Fact]
        public async Task GetProductSize_WithNotExistingProductSize_ReturnsNotFound()
        {
            //Arrage
            int productId = 2;
            A.CallTo(() => mockRepository.IsExist(productId))
                .Returns(false);
            var controller = new ProductSizesController(mockRepository);

            //Act
            var result = await controller.GetProductSize(productId);

            //Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetProductSize_WithExistingProductSize_ReturnsExpectedProductSize()
        {
            //Arrage
            var expectedProductSize = A.Dummy<ProductSize>();
            A.CallTo(() => mockRepository.IsExist(expectedProductSize.Id))
                .Returns(true);
            A.CallTo(() => mockRepository.GetByIdAsync(expectedProductSize.Id))
                .Returns(Task.FromResult(expectedProductSize));
            var controller = new ProductSizesController(mockRepository);

            //Act
            var result = await controller.GetProductSize(expectedProductSize.Id);
            var okResult = result.Result as ObjectResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(expectedProductSize.AsDto());
        }

        [Fact]
        public async Task PutProductSize_WithWrongProductSizeId_ReturnsBadRequest()
        {
            //Arrage
            var ProductSizeToUpdate = A.Dummy<ProductSizeDto>();
            var ProductSizeId = ProductSizeToUpdate.Id + 1;
            var controller = new ProductSizesController(mockRepository);

            //Act
            var result = await controller.PutProductSize(ProductSizeId, ProductSizeToUpdate);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PutProductSize_WithNotExistingProductSize_ReturnsNotFound()
        {
            //Arrage
            var ProductSizeToUpdate = A.Dummy<ProductSizeDto>();
            A.CallTo(() => mockRepository.IsExist(ProductSizeToUpdate.Id))
                .Returns(false);
            var ProductSizeId = ProductSizeToUpdate.Id;
            var controller = new ProductSizesController(mockRepository);

            //Act
            var result = await controller.PutProductSize(ProductSizeId, ProductSizeToUpdate);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task PutProductSize_WithExistingProductSize_ReturnsNoContent()
        {
            //Arrage
            var ProductSizeToUpdate = A.Dummy<ProductSizeDto>();
            A.CallTo(() => mockRepository.IsExist(ProductSizeToUpdate.Id))
                .Returns(true);
            var ProductSizeId = ProductSizeToUpdate.Id;
            var controller = new ProductSizesController(mockRepository);

            //Act
            var result = await controller.PutProductSize(ProductSizeId, ProductSizeToUpdate);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task PostProductSize_WithProductSizeToCreate_ReturnsCreatedProductSize()
        {
            //Arrage
            var ProductSizeToCreate = A.Dummy<PostProductSizeDto>();
            var controller = new ProductSizesController(mockRepository);

            //Act
            var result = await controller.PostProductSize(ProductSizeToCreate);
            var okResult = result.Result as CreatedAtActionResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(ProductSizeToCreate);
        }

        [Fact]
        public async Task DeleteProductSize_WithNotExistingProductSize_ReturnsNotFound()
        {
            //Arrage
            var providedProductSizeId = 1;
            A.CallTo(() => mockRepository.IsExist(providedProductSizeId))
                .Returns(false);
            var controller = new ProductSizesController(mockRepository);

            //Act
            var result = await controller.DeleteProductSize(providedProductSizeId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteProductSize_WithExistingProductSize_ReturnsNoContent()
        {
            //Arrage
            var existingProductSize = A.Dummy<ProductSize>();
            A.CallTo(() => mockRepository.IsExist(existingProductSize.Id))
                .Returns(true);
            var controller = new ProductSizesController(mockRepository);

            //Act
            var result = await controller.DeleteProductSize(existingProductSize.Id);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
