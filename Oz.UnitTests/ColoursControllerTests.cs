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
    public class ColoursControllerTests
    {
        private readonly IColourRepository mockRepository = A.Fake<IColourRepository>();

        [Fact]
        public async Task GetColours_WithOutProductId_ReturnsAllColours()
        {
            //Arrage
            var expectedColours = (List<Colour>)A.CollectionOfDummy<Colour>(5);
            A.CallTo(() => mockRepository.GetAllAsync())
                .Returns(Task.FromResult(expectedColours));
            var controller = new ColoursController(mockRepository);

            //Act
            var result = await controller.GetColours();
            var okResult = result.Result as ObjectResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(expectedColours.Select(colour => colour.AsDto()));
        }

        [Fact]
        public async Task GetColours_WithProductId_ReturnsAllColours()
        {
            //Arrage
            int productId = 2;
            var expectedColours = (List<Colour>)A.CollectionOfDummy<Colour>(5);
            A.CallTo(() => mockRepository.GetAllProductColoursAsync(productId))
                .Returns(Task.FromResult(expectedColours));
            var controller = new ColoursController(mockRepository);

            //Act
            var result = await controller.GetColours(productId);
            var okResult = result.Result as ObjectResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(expectedColours.Select(colour => colour.AsDto()));
        }

        [Fact]
        public async Task GetColour_WithNotExistingColour_ReturnsNotFound()
        {
            //Arrage
            int productId = 2;
            A.CallTo(() => mockRepository.IsExist(productId))
                .Returns(false);
            var controller = new ColoursController(mockRepository);

            //Act
            var result = await controller.GetColour(productId);

            //Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetColour_WithExistingColour_ReturnsExpectedColour()
        {
            //Arrage
            var expectedColour = A.Dummy<Colour>();
            A.CallTo(() => mockRepository.IsExist(expectedColour.Id))
                .Returns(true);
            A.CallTo(() => mockRepository.GetByIdAsync(expectedColour.Id)).Returns(Task.FromResult(expectedColour));
            var controller = new ColoursController(mockRepository);

            //Act
            var result = await controller.GetColour(expectedColour.Id);
            var okResult = result.Result as ObjectResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(expectedColour.AsDto());
        }

        [Fact]
        public async Task PutColour_WithWrongColourId_ReturnsBadRequest()
        {
            //Arrage
            var colourToUpdate = A.Dummy<ColourDto>();
            var colourId = colourToUpdate.Id + 1;
            var controller = new ColoursController(mockRepository);

            //Act
            var result = await controller.PutColour(colourId, colourToUpdate);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PutColour_WithNotExistingColour_ReturnsNotFound()
        {
            //Arrage
            var colourToUpdate = A.Dummy<ColourDto>();
            A.CallTo(() => mockRepository.IsExist(colourToUpdate.Id))
                .Returns(false);
            var colourId = colourToUpdate.Id;
            var controller = new ColoursController(mockRepository);

            //Act
            var result = await controller.PutColour(colourId, colourToUpdate);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task PutColour_WithExistingColour_ReturnsNoContent()
        {
            //Arrage
            var colourToUpdate = A.Dummy<ColourDto>();
            A.CallTo(() => mockRepository.IsExist(colourToUpdate.Id))
                .Returns(true);
            var colourId = colourToUpdate.Id;
            var controller = new ColoursController(mockRepository);

            //Act
            var result = await controller.PutColour(colourId, colourToUpdate);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task PostColour_WithColourToCreate_ReturnsCreatedColour()
        {
            //Arrage
            var colourToCreate = A.Dummy<PostColourDto>();
            var controller = new ColoursController(mockRepository);

            //Act
            var result = await controller.PostColour(colourToCreate);
            var okResult = result.Result as CreatedAtActionResult;

            //Assert
            okResult.Value.Should().BeEquivalentTo(colourToCreate);
        }

        [Fact]
        public async Task DeleteColour_WithNotExistingColour_ReturnsNotFound()
        {
            //Arrage
            var providedColourId = 1;
            A.CallTo(() => mockRepository.IsExist(providedColourId))
                .Returns(false);
            var controller = new ColoursController(mockRepository);

            //Act
            var result = await controller.DeleteColour(providedColourId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteColour_WithExistingColour_ReturnsNoContent()
        {
            //Arrage
            var existingColour = A.Dummy<Colour>();
            A.CallTo(() => mockRepository.IsExist(existingColour.Id))
                .Returns(true);
            var controller = new ColoursController(mockRepository);

            //Act
            var result = await controller.DeleteColour(existingColour.Id);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
