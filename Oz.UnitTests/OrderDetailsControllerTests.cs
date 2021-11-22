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
    public class OrderDetailsControllerTests
    {
        private readonly IOrderDetailRepository mockRepository = A.Fake<IOrderDetailRepository>();

        [Fact]
        public async Task GetOrderDetails_WithOutOrderId_ReturnsBadRequest()
        {
            //Arrage            
            var controller = new OrderDetailsController(mockRepository);

            //Act
            var result = await controller.GetOrderDetails();

            //Assert
            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task GetOrderDetails_WithOrderId_ReturnsAllOrderDetails()
        {
            //Arrage
            int orderId = 2;
            var expectedOrderDetails = (List<OrderDetail>)A.CollectionOfDummy<OrderDetail>(5);
            A.CallTo(() => mockRepository.GetAllAsync(orderId))
                .Returns(Task.FromResult(expectedOrderDetails));
            var controller = new OrderDetailsController(mockRepository);

            //Act
            var result = await controller.GetOrderDetails(orderId);
            var okResult = result.Result as ObjectResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(expectedOrderDetails.Select(orderDetail => orderDetail.AsDto()));
        }

        [Fact]
        public async Task GetOrderDetail_WithNotExistingOrderDetail_ReturnsNotFound()
        {
            //Arrage
            int orderDetailId = 2;
            A.CallTo(() => mockRepository.IsExist(orderDetailId))
                .Returns(false);
            var controller = new OrderDetailsController(mockRepository);

            //Act
            var result = await controller.GetOrderDetail(orderDetailId);

            //Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetOrderDetail_WithExistingOrderDetail_ReturnsExpectedOrderDetail()
        {
            //Arrage
            var expectedOrderDetail = A.Dummy<OrderDetail>();
            A.CallTo(() => mockRepository.IsExist(expectedOrderDetail.Id))
                .Returns(true);
            A.CallTo(() => mockRepository.GetByIdAsync(expectedOrderDetail.Id)).Returns(Task.FromResult(expectedOrderDetail));
            var controller = new OrderDetailsController(mockRepository);

            //Act
            var result = await controller.GetOrderDetail(expectedOrderDetail.Id);
            var okResult = result.Result as ObjectResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(expectedOrderDetail.AsDto());
        }

        [Fact]
        public async Task PutOrderDetail_WithWrongOrderDetailId_ReturnsBadRequest()
        {
            //Arrage
            var OrderDetailToUpdate = A.Dummy<OrderDetailDto>();
            var OrderDetailId = OrderDetailToUpdate.Id + 1;
            var controller = new OrderDetailsController(mockRepository);

            //Act
            var result = await controller.PutOrderDetail(OrderDetailId, OrderDetailToUpdate);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PutOrderDetail_WithNotExistingOrderDetail_ReturnsNotFound()
        {
            //Arrage
            var OrderDetailToUpdate = A.Dummy<OrderDetailDto>();
            A.CallTo(() => mockRepository.IsExist(OrderDetailToUpdate.Id))
                .Returns(false);
            var OrderDetailId = OrderDetailToUpdate.Id;
            var controller = new OrderDetailsController(mockRepository);

            //Act
            var result = await controller.PutOrderDetail(OrderDetailId, OrderDetailToUpdate);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task PutOrderDetail_WithExistingOrderDetail_ReturnsNoContent()
        {
            //Arrage
            var OrderDetailToUpdate = A.Dummy<OrderDetailDto>();
            A.CallTo(() => mockRepository.IsExist(OrderDetailToUpdate.Id))
                .Returns(true);
            var OrderDetailId = OrderDetailToUpdate.Id;
            var controller = new OrderDetailsController(mockRepository);

            //Act
            var result = await controller.PutOrderDetail(OrderDetailId, OrderDetailToUpdate);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task PostOrderDetail_WithOrderDetailToCreate_ReturnsCreatedOrderDetail()
        {
            //Arrage
            var orderDetailToCreate = A.Dummy<PostOrderDetailDto>();
            var controller = new OrderDetailsController(mockRepository);

            //Act
            var result = await controller.PostOrderDetail(orderDetailToCreate);
            var okResult = result.Result as CreatedAtActionResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(orderDetailToCreate);

        }

        [Fact]
        public async Task DeleteOrderDetail_WithNotExistingOrderDetail_ReturnsNotFound()
        {
            //Arrage
            var providedOrderDetailId = 1;
            A.CallTo(() => mockRepository.IsExist(providedOrderDetailId))
                .Returns(false);
            var controller = new OrderDetailsController(mockRepository);

            //Act
            var result = await controller.DeleteOrderDetail(providedOrderDetailId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteOrderDetail_WithExistingOrderDetail_ReturnsNoContent()
        {
            //Arrage
            var existingOrderDetail = A.Dummy<OrderDetail>();
            A.CallTo(() => mockRepository.IsExist(existingOrderDetail.Id))
                .Returns(true);
            var controller = new OrderDetailsController(mockRepository);

            //Act
            var result = await controller.DeleteOrderDetail(existingOrderDetail.Id);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
