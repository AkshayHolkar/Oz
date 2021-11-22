using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oz.Controllers.V1;
using Oz.Domain;
using Oz.Dtos;
using Oz.Extensions;
using Oz.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            var expectedProducts = (List<Product>)A.CollectionOfDummy<Product>(5);
            A.CallTo(() => mockRepository.GetAllAsync())
                .Returns(expectedProducts);
            var controller = GetController();

            //Act
            var result = await controller.GetProducts();
            var okResult = result.Result as ObjectResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(expectedProducts.Select(product => product.AsDto()));
        }

        [Fact]
        public async Task GetProduct_WithNotExistingProduct_ReturnsNotFound()
        {
            //Arrage
            int productId = 2;
            A.CallTo(() => mockRepository.GetByIdAsync(productId))
                .Returns(Task.FromResult((Product)null));
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
            int productId = 2;
            var expectedProduct = A.Dummy<Product>();
            expectedProduct.Id = productId;
            A.CallTo(() => mockRepository.GetByIdAsync(productId)).Returns(expectedProduct);
            var controller = GetController();


            //Act
            var result = await controller.GetProduct(productId);
            var okResult = result.Result as ObjectResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(expectedProduct.AsDto());
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
            var productToCreate = A.Dummy<PostProductDto>();
            var controller = GetController();

            //Act
            var result = await controller.PostProduct(productToCreate);
            var okResult = result.Result as CreatedAtActionResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(productToCreate);
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

        private ProductsController GetController()
        {
            ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "Approved")
            }, "mock"));
            var controller = new ProductsController(mockRepository);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            return controller;
        }
    }
}
