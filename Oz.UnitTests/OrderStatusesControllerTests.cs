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
    public class OrderStatusesControllerTests
    {
        private readonly IDomainsRepository<OrderStatus> mockRepository = A.Fake<IDomainsRepository<OrderStatus>>();

        [Fact]
        public async Task GetOrderStatuses_WithExistingOrderStatuses_ReturnsAllOrderStatuses()
        {
            //Arrage
            var expectedOrderStatuses = (List<OrderStatus>)A.CollectionOfDummy<OrderStatus>(5);
            A.CallTo(() => mockRepository.GetAllAsync())
                .Returns(Task.FromResult(expectedOrderStatuses));
            var controller = new OrderStatusesController(mockRepository);

            //Act
            var result = await controller.GetOrderStatuses();

            //Assert
            result.Value.Should().BeEquivalentTo(expectedOrderStatuses);
        }

        [Fact]
        public async Task GetOrderStatus_WithNotExistingOrderStatus_ReturnsNotFound()
        {
            //Arrage
            A.CallTo(() => mockRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((OrderStatus)null));
            var controller = new OrderStatusesController(mockRepository);

            //Act
            var result = await controller.GetOrderStatus(It.IsAny<int>());

            //Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetOrderStatus_WithExistingOrderStatus_ReturnsExpectedOrderStatus()
        {
            //Arrage
            var expectedOrderStatus = A.Dummy<OrderStatus>();
            A.CallTo(() => mockRepository.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(expectedOrderStatus));
            var controller = new OrderStatusesController(mockRepository);

            //Act
            var result = await controller.GetOrderStatus(It.IsAny<int>());

            //Assert
            result.Value.Should().BeEquivalentTo(expectedOrderStatus);
        }

        [Fact]
        public async Task PutOrderStatus_WithWrongOrderStatusId_ReturnsBadRequest()
        {
            //Arrage
            var orderStatusToUpdate = A.Dummy<OrderStatus>();
            var orderStatusId = orderStatusToUpdate.Id + 1;
            var controller = new OrderStatusesController(mockRepository);

            //Act
            var result = await controller.PutOrderStatus(orderStatusId, orderStatusToUpdate);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PutOrderStatus_WithNotExistingOrderStatus_ReturnsNotFound()
        {
            //Arrage
            var orderStatusToUpdate = A.Dummy<OrderStatus>();
            A.CallTo(() => mockRepository.IsExist(orderStatusToUpdate.Id))
                .Returns(false);
            var orderStatusId = orderStatusToUpdate.Id;
            var controller = new OrderStatusesController(mockRepository);

            //Act
            var result = await controller.PutOrderStatus(orderStatusId, orderStatusToUpdate);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task PutOrderStatus_WithExistingOrderStatus_ReturnsNoContent()
        {
            //Arrage
            var orderStatusToUpdate = A.Dummy<OrderStatus>();
            A.CallTo(() => mockRepository.IsExist(orderStatusToUpdate.Id))
                .Returns(true);
            var orderStatusId = orderStatusToUpdate.Id;
            var controller = new OrderStatusesController(mockRepository);

            //Act
            var result = await controller.PutOrderStatus(orderStatusId, orderStatusToUpdate);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task PostOrderStatus_WithOrderStatusToCreate_ReturnsCreatedOrderStatus()
        {
            //Arrage
            var orderStatusToCreate = A.Dummy<PostOrderStatusDto>();
            var controller = new OrderStatusesController(mockRepository);

            //Act
            var result = await controller.PostOrderStatus(orderStatusToCreate);

            //Assert
            var createdOrderStatus = (result.Result as CreatedAtActionResult).Value as OrderStatus;
            result.Result.Should().BeEquivalentTo(
                createdOrderStatus,
                Options => Options.ComparingByMembers<Category>().ExcludingMissingMembers()
                );
        }

        [Fact]
        public async Task DeleteOrderStatus_WithNotExistingOrderStatus_ReturnsNotFound()
        {
            //Arrage
            var providedOrderStatusId = 1;
            A.CallTo(() => mockRepository.IsExist(providedOrderStatusId))
                .Returns(false);
            var controller = new OrderStatusesController(mockRepository);

            //Act
            var result = await controller.DeleteOrderStatus(providedOrderStatusId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteOrderStatus_WithExistingOrderStatus_ReturnsNoContent()
        {
            //Arrage
            var existingOrderStatus = A.Dummy<OrderStatus>();
            A.CallTo(() => mockRepository.IsExist(existingOrderStatus.Id))
                .Returns(true);
            var controller = new OrderStatusesController(mockRepository);

            //Act
            var result = await controller.DeleteOrderStatus(existingOrderStatus.Id);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
