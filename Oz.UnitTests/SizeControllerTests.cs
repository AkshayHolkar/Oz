using Microsoft.AspNetCore.Mvc;
using Moq;
using Oz.Controllers.V1;
using Oz.Domain;
using Oz.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Oz.UnitTests
{
    public class SizeControllerTests
    {
        [Fact]
        public async Task GetSize_WithUnexistingItem_ReturnsNotFound()
        {
            //Arrage
            var repositoryStub = new Mock<IDomainsRepository<Size>>();
            repositoryStub.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Size)null);
            var controller = new SizesController(repositoryStub.Object);

            //Act
            var result = await controller.GetSize(It.IsAny<int>());

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
