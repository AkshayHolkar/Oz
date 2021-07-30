using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Oz.Controllers.V1;
using Oz.Domain;
using Oz.Dtos;
using Oz.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Oz.UnitTests
{
    public class SizeControllerTests
    {
        private readonly IDomainsRepository<Size> mockRepository = A.Fake<IDomainsRepository<Size>>();

        [Fact]
        public async Task GetSizes_WithExistingSizes_ReturnsAllSizes()
        {
            //Arrage
            var expectedSizes = (List<Size>)A.CollectionOfDummy<Size>(5);
            A.CallTo(() => mockRepository.GetAllAsync())
                .Returns(Task.FromResult(expectedSizes));
            var controller = new SizesController(mockRepository);

            //Act
            var result = await controller.GetSizes();

            //Assert
            result.Value.Should().BeEquivalentTo(expectedSizes);
        }

        [Fact]
        public async Task GetSize_WithNotExistingSize_ReturnsNotFound()
        {
            //Arrage
            A.CallTo(() => mockRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((Size)null));
            var controller = new SizesController(mockRepository);

            //Act
            var result = await controller.GetSize(It.IsAny<int>());

            //Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetSize_WithExistingSize_ReturnsExpectedSize()
        {
            //Arrage
            var expectedSize = A.Dummy<Size>();
            A.CallTo(() => mockRepository.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(expectedSize));
            var controller = new SizesController(mockRepository);

            //Act
            var result = await controller.GetSize(It.IsAny<int>());

            //Assert
            result.Value.Should().BeEquivalentTo(expectedSize);
        }

        [Fact]
        public async Task PutSizeAsync_WithWrongSizeId_ReturnsBadRequest()
        {
            //Arrage
            var sizeToUpdate = A.Dummy<Size>();
            var sizeId = sizeToUpdate.Id + 1;
            var controller = new SizesController(mockRepository);

            //Act
            var result = await controller.PutSizeAsync(sizeId, sizeToUpdate);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PutSizeAsync_WithNotExistingSize_ReturnsNotFound()
        {
            //Arrage
            var sizeToUpdate = A.Dummy<Size>();
            A.CallTo(() => mockRepository.IsExist(sizeToUpdate.Id))
                .Returns(false);
            var sizeId = sizeToUpdate.Id;
            var controller = new SizesController(mockRepository);

            //Act
            var result = await controller.PutSizeAsync(sizeId, sizeToUpdate);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task PutSizeAsync_WithExistingSize_ReturnsNoContent()
        {
            //Arrage
            var sizeToUpdate = A.Dummy<Size>();
            A.CallTo(() => mockRepository.IsExist(sizeToUpdate.Id))
                .Returns(true);
            var sizeId = sizeToUpdate.Id;
            var controller = new SizesController(mockRepository);

            //Act
            var result = await controller.PutSizeAsync(sizeId, sizeToUpdate);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task PostSize_WithSizeToCreate_ReturnsCreatedSize()
        {
            //Arrage
            var sizeToCreate = A.Dummy<PostSizeDto>();
            var controller = new SizesController(mockRepository);

            //Act
            var result = await controller.PostSize(sizeToCreate);

            //Assert
            var createdSize = (result.Result as CreatedAtActionResult).Value as Size;
            result.Result.Should().BeEquivalentTo(
                createdSize,
                Options => Options.ComparingByMembers<Size>().ExcludingMissingMembers()
                );
        }

        [Fact]
        public async Task DeleteSizeAsync_WithNotExistingSize_ReturnsNotFound()
        {
            //Arrage
            var providedSizeId = 1;
            A.CallTo(() => mockRepository.IsExist(providedSizeId))
                .Returns(false);
            var controller = new SizesController(mockRepository);

            //Act
            var result = await controller.DeleteSizeAsync(providedSizeId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteSizeAsync_WithExistingSize_ReturnsNoContent()
        {
            //Arrage
            var existingSize = A.Dummy<Size>();
            A.CallTo(() => mockRepository.IsExist(existingSize.Id))
                .Returns(true);
            var controller = new SizesController(mockRepository);

            //Act
            var result = await controller.DeleteSizeAsync(existingSize.Id);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
