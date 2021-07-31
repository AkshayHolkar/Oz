using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Oz.Controllers.V1;
using Oz.Domain;
using Oz.Dtos;
using Oz.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Oz.UnitTests
{
    public class ProductsControllerTests
    {
        private readonly IProductRepository mockRepository = A.Fake<IProductRepository>();

        [Fact]
        public async Task GetProducts_WithExistingProducts_ReturnsAllProducts()
        {
            //Arrage
            var expectedProducts = (List<ProductDto>)A.CollectionOfDummy<ProductDto>(5);
            A.CallTo(() => mockRepository.GetAllAsync())
                .Returns(Task.FromResult(expectedProducts));
            var controller = new ProductsController(mockRepository);

            //Act
            var result = await controller.GetProducts();

            //Assert
            result.Value.Should().BeEquivalentTo(expectedProducts);
        }

        [Fact]
        public async Task GetProduct_WithNotExistingProduct_ReturnsNotFound()
        {
            //Arrage
            var productId = 2;
            A.CallTo(() => mockRepository.GetByIdAsync(productId))
                .Returns(Task.FromResult((ProductDto)null));
            var controller = new ProductsController(mockRepository);

            //Act
            var result = await controller.GetProduct(productId);

            //Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetProduct_WithExistingProduct_ReturnsExpectedProduct()
        {
            //Arrage
            var expectedProduct = A.Dummy<ProductDto>();
            A.CallTo(() => mockRepository.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(expectedProduct));
            var controller = new ProductsController(mockRepository);

            //Act
            var result = await controller.GetProduct(2);

            //Assert
            result.Value.Should().BeEquivalentTo(expectedProduct);
        }

        [Fact]
        public async Task PutProduct_WithWrongProductId_ReturnsBadRequest()
        {
            //Arrage
            var productToUpdate = A.Dummy<ProductDto>();
            var ProductId = productToUpdate.Id + 1;
            var controller = new ProductsController(mockRepository);

            //Act
            var result = await controller.PutProduct(ProductId, productToUpdate);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PutProduct_WithNotExistingProduct_ReturnsNotFound()
        {
            //Arrage
            var productToUpdate = A.Dummy<ProductDto>();
            A.CallTo(() => mockRepository.IsExist(productToUpdate.Id))
                .Returns(false);
            var ProductId = productToUpdate.Id;
            var controller = new ProductsController(mockRepository);

            //Act
            var result = await controller.PutProduct(ProductId, productToUpdate);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task PutProduct_WithExistingProduct_ReturnsNoContent()
        {
            //Arrage
            var productToUpdate = A.Dummy<ProductDto>();
            A.CallTo(() => mockRepository.IsExist(productToUpdate.Id))
                .Returns(true);
            var ProductId = productToUpdate.Id;
            var controller = new ProductsController(mockRepository);

            //Act
            var result = await controller.PutProduct(ProductId, productToUpdate);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task PostProduct_WithProductToCreate_ReturnsCreatedProduct()
        {
            //Arrage
            var ProductToCreate = A.Dummy<PostProductDto>();
            var controller = new ProductsController(mockRepository);

            //Act
            var result = await controller.PostProduct(ProductToCreate);

            //Assert
            var createdProduct = (result.Result as CreatedAtActionResult).Value as ProductDto;
            result.Result.Should().BeEquivalentTo(
                createdProduct,
                Options => Options.ComparingByMembers<PostProductDto>().ExcludingMissingMembers()
                );
        }

        [Fact]
        public async Task DeleteProduct_WithNotExistingProduct_ReturnsNotFound()
        {
            //Arrage
            var providedProductId = 1;
            A.CallTo(() => mockRepository.IsExist(providedProductId))
                .Returns(false);
            var controller = new ProductsController(mockRepository);

            //Act
            var result = await controller.DeleteProduct(providedProductId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteProduct_WithExistingProduct_ReturnsNoContent()
        {
            //Arrage
            var existingProduct = A.Dummy<Product>();
            A.CallTo(() => mockRepository.IsExist(existingProduct.Id))
                .Returns(true);
            var controller = new ProductsController(mockRepository);

            //Act
            var result = await controller.DeleteProduct(existingProduct.Id);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
