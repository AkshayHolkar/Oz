using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Oz.Controllers.V1;
using Oz.Extensions;
using Oz.Dtos;
using Oz.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Security.Claims;
using Oz.Repositories.Contracts;
using Oz.Domain;
using System.Linq;

namespace Oz.UnitTests
{
    public class CartsControllerTests
    {
        private readonly ICartRepository mockRepository = A.Fake<ICartRepository>();
        private readonly ISharedService sharedService = A.Fake<ISharedService>();

        [Fact]
        public async Task GetCarts_WithExistingCarts_ReturnsAllCarts()
        {
            //Arrage
            var controller = GetController();
            var expectedCarts = (List<Cart>)A.CollectionOfDummy<Cart>(5);
            A.CallTo(() => mockRepository.GetAllAsync(controller.HttpContext.GetUserId()))
                .Returns(Task.FromResult(expectedCarts));

            //Act
            var result = await controller.GetCarts();
            var okResult = result.Result as ObjectResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(expectedCarts.Select(cart => cart.AsDto()));
        }

        [Fact]
        public async Task GetCart_WithNotExistingCart_ReturnsNotFound()
        {
            //Arrage
            var controller = GetController();
            var cartId = 2;
            A.CallTo(() => mockRepository.IsExist(cartId))
                .Returns(false);

            //Act
            var result = await controller.GetCart(cartId);

            //Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetCart_WithNotCartOwner_ReturnsBadRequest()
        {
            //Arrage
            var controller = GetController();
            var existingCart = A.Dummy<Cart>();
            A.CallTo(() => mockRepository.IsExist(It.IsAny<int>()))
                .Returns(true);
            A.CallTo(() => mockRepository.GetByIdAsync(existingCart.Id))
                .Returns(Task.FromResult(existingCart));
            A.CallTo(() => sharedService.UserOwnsDomain(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            //Act
            var result = await controller.GetCart(existingCart.Id);

            //Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetCart_WithExistingCart_ReturnsExpectedCart()
        {
            //Arrage
            var controller = GetController();
            var expectedCart = A.Dummy<Cart>();
            expectedCart.CustomerId = controller.HttpContext.GetUserId();
            A.CallTo(() => mockRepository.IsExist(expectedCart.Id))
                .Returns(true);
            A.CallTo(() => mockRepository.GetByIdAsync(expectedCart.Id))
                .Returns(Task.FromResult(expectedCart));
            A.CallTo(() => sharedService.UserOwnsDomain(expectedCart.CustomerId, controller.HttpContext.GetUserId()))
                .Returns(true);

            //Act
            var result = await controller.GetCart(expectedCart.Id);
            var okResult = result.Result as ObjectResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(expectedCart.AsDto());
        }

        [Fact]
        public async Task PutCart_WithWrongId_ReturnsBadRequest()
        {
            //Arrage
            var controller = GetController();
            var CartToUpdate = A.Dummy<CartDto>();
            var id = CartToUpdate.Id + 1;

            //Act
            var result = await controller.PutCart(id, CartToUpdate);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PutCart_WithNotExistingCart_ReturnsNotFound()
        {
            //Arrage
            var controller = GetController();
            var CartToUpdate = A.Dummy<CartDto>();
            A.CallTo(() => mockRepository.IsExist(CartToUpdate.Id))
                .Returns(false);
            var userId = CartToUpdate.Id;

            //Act
            var result = await controller.PutCart(userId, CartToUpdate);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task PutCart_WithNotCartOwner_ReturnsBadRequest()
        {
            //Arrage
            var controller = GetController();
            var cartToUpdate = A.Dummy<CartDto>();
            A.CallTo(() => mockRepository.IsExist(cartToUpdate.Id))
                .Returns(true);
            var userId = cartToUpdate.Id;

            //Act
            var result = await controller.PutCart(userId, cartToUpdate);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task PutCart_WithExistingCart_ReturnsNoContent()
        {
            //Arrage
            var controller = GetController();
            var cartToUpdate = A.Dummy<CartDto>();
            A.CallTo(() => mockRepository.IsExist(cartToUpdate.Id))
                .Returns(true);
            A.CallTo(() => sharedService.UserOwnsDomain(cartToUpdate.CustomerId, controller.HttpContext.GetUserId()))
                .Returns(true);
            var userId = cartToUpdate.Id;

            //Act
            var result = await controller.PutCart(userId, cartToUpdate);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task PostCart_WithCartToCreate_ReturnsCreatedCart()
        {
            //Arrage
            var controller = GetController();
            var cartToCreate = A.Dummy<CreateCartDto>();

            //Act
            var result = await controller.PostCart(cartToCreate);
            var okResult = result.Result as CreatedAtActionResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(cartToCreate);
        }

        [Fact]
        public async Task DeleteCart_WithNotExistingCart_ReturnsNotFound()
        {
            //Arrage
            var controller = GetController();
            var providedCartId = 1;
            A.CallTo(() => mockRepository.IsExist(providedCartId))
                .Returns(false);

            //Act
            var result = await controller.DeleteCart(providedCartId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteCart_WithNotCartOwner_ReturnsBadRequest()
        {
            //Arrage
            var controller = GetController();
            var existingCart = A.Dummy<Cart>();
            existingCart.CustomerId = controller.HttpContext.GetUserId();
            A.CallTo(() => mockRepository.IsExist(existingCart.Id))
                .Returns(true);
            A.CallTo(() => mockRepository.GetByIdAsync(existingCart.Id))
                .Returns(existingCart);
            A.CallTo(() => sharedService.UserOwnsDomain(existingCart.CustomerId, controller.HttpContext.GetUserId()))
                .Returns(false);

            //Act
            var result = await controller.DeleteCart(existingCart.Id);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteCart_WithExistingCart_ReturnsNoContent()
        {
            //Arrage
            var controller = GetController();
            var existingCart = A.Dummy<Cart>();
            existingCart.CustomerId = controller.HttpContext.GetUserId();
            A.CallTo(() => mockRepository.IsExist(existingCart.Id))
                .Returns(true);
            A.CallTo(() => mockRepository.GetByIdAsync(existingCart.Id))
                .Returns(existingCart);
            A.CallTo(() => sharedService.UserOwnsDomain(existingCart.CustomerId, controller.HttpContext.GetUserId()))
                .Returns(true);

            //Act
            var result = await controller.DeleteCart(existingCart.Id);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        private CartsController GetController()
        {
            ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("id", "1256asda"),
            }, "mock"));
            var controller = new CartsController(mockRepository, sharedService);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            return controller;
        }
    }
}
