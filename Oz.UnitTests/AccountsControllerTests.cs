using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Oz.Controllers.V1;
using Oz.Extensions;
using Oz.Dtos;
using Oz.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Security.Claims;
using Oz.Repositories.Contracts;
using Oz.Domain;
using System.Linq;

namespace Oz.UnitTests
{
    public class AccountsControllerTests
    {
        private readonly IAccountRepository mockRepository = A.Fake<IAccountRepository>();
        private readonly IIdentityService identityService = A.Fake<IIdentityService>();
        private readonly ISharedService sharedService = A.Fake<ISharedService>();

        [Fact]
        public async Task GetAccounts_WithExistingAccounts_ReturnsAllAccounts()
        {
            //Arrage
            var controller = GetController();
            var expectedAccounts = (List<Account>)A.CollectionOfDummy<Account>(5);
            A.CallTo(() => mockRepository.GetAllAsync())
                .Returns(Task.FromResult(expectedAccounts));

            //Act
            var result = await controller.GetAccounts(false, false);
            var okResult = result.Result as ObjectResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(expectedAccounts.Select(account => account.AsDto()));
        }

        [Fact]
        public async Task GetAccount_WithNotExistingOwnerAccount_ReturnsNotFound()
        {
            //Arrage
            var controller = GetController();
            A.CallTo(() => mockRepository.IsExist(It.IsAny<string>()))
                .Returns(false);

            //Act
            var result = await controller.GetAccount();

            //Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetAccount_WithExistingOwnerAccount_ReturnsNotFound()
        {
            //Arrage
            var controller = GetController();
            A.CallTo(() => mockRepository.IsExist(It.IsAny<string>()))
                .Returns(true);

            //Act
            var result = await controller.GetAccount();

            //Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetAccount_WithNotExistingAccount_ReturnsNotFound()
        {
            //Arrage
            var controller = GetController();
            A.CallTo(() => mockRepository.IsExist(It.IsAny<string>()))
                .Returns(false);

            //Act
            var result = await controller.GetAccount(Guid.NewGuid().ToString());

            //Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetAccount_WithExistingAccount_ReturnsExpectedAccount()
        {
            //Arrage
            var controller = GetController();
            var expectedAccount = A.Dummy<Account>();
            expectedAccount.UserId = "1256asda";
            A.CallTo(() => mockRepository.IsExist(expectedAccount.UserId))
                .Returns(true);
            A.CallTo(() => mockRepository.GetByIdAsync(expectedAccount.UserId))
                .Returns(Task.FromResult(expectedAccount));

            //Act
            var result = await controller.GetAccount(expectedAccount.UserId);
            var okResult = result.Result as ObjectResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(expectedAccount.AsDto());
        }

        [Fact]
        public async Task PutAccount_WithWrongId_ReturnsBadRequest()
        {
            //Arrage
            var controller = GetController();
            var accountToUpdate = A.Dummy<AccountDto>();
            var id = accountToUpdate.UserId + 1;

            //Act
            var result = await controller.PutAccount(id, accountToUpdate);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PutAccount_WithNotExistingAccount_ReturnsNotFound()
        {
            //Arrage
            var controller = GetController();
            var accountToUpdate = A.Dummy<AccountDto>();
            A.CallTo(() => mockRepository.IsExist(accountToUpdate.UserId))
                .Returns(false);
            var userId = accountToUpdate.UserId;

            //Act
            var result = await controller.PutAccount(userId, accountToUpdate);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task PutAccount_WithExistingAccount_ReturnsNoContent()
        {
            //Arrage
            var controller = GetController();
            var accountToUpdate = A.Dummy<AccountDto>();
            A.CallTo(() => mockRepository.IsExist(accountToUpdate.UserId))
                .Returns(true);
            var userId = accountToUpdate.UserId;
            A.CallTo(() => identityService.IsAdminAsync(controller.HttpContext.GetUserId()))
                .Returns(true);

            //Act
            var result = await controller.PutAccount(userId, accountToUpdate);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task PostAccount_WithAccountToCreate_ReturnsCreatedAccount()
        {
            //Arrage
            var controller = GetController();
            var accountToCreate = A.Dummy<CreateAccountDto>();

            //Act
            var result = await controller.PostAccount(accountToCreate);
            var okResult = result.Result as CreatedAtActionResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(accountToCreate);
        }

        [Fact]
        public async Task DeleteAccount_WithNotExistingAccount_ReturnsNotFound()
        {
            //Arrage
            var controller = GetController();
            var providedAccountId = "a3f4tgv567thyjty6";
            A.CallTo(() => mockRepository.IsExist(providedAccountId))
                .Returns(false);

            //Act
            var result = await controller.DeleteAccount(providedAccountId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteAccount_WithNotAccountOwner_ReturnsBadRequest()
        {
            //Arrage
            var controller = GetController();
            var existingAccount = A.Dummy<Account>();
            existingAccount.UserId = controller.HttpContext.GetUserId();
            A.CallTo(() => mockRepository.IsExist(existingAccount.UserId))
                .Returns(true);
            A.CallTo(() => mockRepository.GetByIdAsync(existingAccount.UserId))
                .Returns(existingAccount);
            A.CallTo(() => sharedService.UserOwnsDomain(existingAccount.UserId, controller.HttpContext.GetUserId()))
                .Returns(false);

            //Act
            var result = await controller.DeleteAccount(existingAccount.UserId);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteAccount_WithExistingAccount_ReturnsNoContent()
        {
            //Arrage
            var controller = GetController();
            var existingAccount = A.Dummy<Account>();
            existingAccount.UserId = controller.HttpContext.GetUserId();
            A.CallTo(() => mockRepository.IsExist(existingAccount.UserId))
                .Returns(true);
            A.CallTo(() => mockRepository.GetByIdAsync(existingAccount.UserId))
                .Returns(existingAccount);
            A.CallTo(() => sharedService.UserOwnsDomain(existingAccount.UserId, controller.HttpContext.GetUserId()))
                .Returns(true);

            //Act
            var result = await controller.DeleteAccount(existingAccount.UserId);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        private AccountsController GetController()
        {
            ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("id", "1256asda"),
            }, "mock"));
            var controller = new AccountsController(mockRepository, identityService, sharedService);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            return controller;
        }
    }
}
