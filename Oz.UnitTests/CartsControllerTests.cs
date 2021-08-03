using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Oz.Controllers.V1;
using Oz.Extensions;
using Oz.Dtos;
using Oz.Repositories;
using Oz.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Security.Claims;

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
            var expectedCarts = (List<CartDto>)A.CollectionOfDummy<CartDto>(5);
            A.CallTo(() => mockRepository.GetAllAsync(controller.HttpContext.GetUserId()))
                .Returns(Task.FromResult(expectedCarts));

            //Act
            var result = await controller.GetCarts();

            //Assert
            result.Value.Should().BeEquivalentTo(expectedCarts);
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
            var existingCart = A.Dummy<CartDto>();
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
            var expectedCart = A.Dummy<CartDto>();
            expectedCart.CustomerId = controller.HttpContext.GetUserId();
            A.CallTo(() => mockRepository.IsExist(expectedCart.Id))
                .Returns(true);
            A.CallTo(() => mockRepository.GetByIdAsync(expectedCart.Id))
                .Returns(Task.FromResult(expectedCart));
            A.CallTo(() => sharedService.UserOwnsDomain(expectedCart.CustomerId, controller.HttpContext.GetUserId()))
                .Returns(true);

            //Act
            var result = await controller.GetCart(expectedCart.Id);

            //Assert
            result.Value.Should().BeEquivalentTo(expectedCart);
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
            var CartToCreate = A.Dummy<CreateCartDto>();

            //Act
            var result = await controller.PostCart(CartToCreate);

            //Assert
            var createdCart = (result.Result as CreatedAtActionResult).Value as CartDto;
            result.Result.Should().BeEquivalentTo(
                createdCart,
                Options => Options.ComparingByMembers<CreateCartDto>().ExcludingMissingMembers()
                );
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
            var existingCart = A.Dummy<CartDto>();
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
            var existingCart = A.Dummy<CartDto>();
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
