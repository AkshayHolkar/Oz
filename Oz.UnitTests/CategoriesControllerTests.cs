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
    public class CategoriesControllerTests
    {
        private readonly IDomainsRepository<Category> mockRepository = A.Fake<IDomainsRepository<Category>>();

        [Fact]
        public async Task GetCategories_WithExistingCategories_ReturnsAllCategories()
        {
            //Arrage
            var expectedCategories = (List<Category>)A.CollectionOfDummy<Category>(5);
            A.CallTo(() => mockRepository.GetAllAsync())
                .Returns(Task.FromResult(expectedCategories));
            var controller = new CategoriesController(mockRepository);

            //Act
            var result = await controller.GetCategories();

            //Assert
            result.Value.Should().BeEquivalentTo(expectedCategories);
        }

        [Fact]
        public async Task GetCategory_WithNotExistingCategory_ReturnsNotFound()
        {
            //Arrage
            A.CallTo(() => mockRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((Category)null));
            var controller = new CategoriesController(mockRepository);

            //Act
            var result = await controller.GetCategory(It.IsAny<int>());

            //Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetCategory_WithExistingCategory_ReturnsExpectedCategory()
        {
            //Arrage
            var expectedCategory = A.Dummy<Category>();
            A.CallTo(() => mockRepository.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(expectedCategory));
            var controller = new CategoriesController(mockRepository);

            //Act
            var result = await controller.GetCategory(It.IsAny<int>());

            //Assert
            result.Value.Should().BeEquivalentTo(expectedCategory);
        }

        [Fact]
        public async Task PutCategory_WithWrongCategoryId_ReturnsBadRequest()
        {
            //Arrage
            var categoryToUpdate = A.Dummy<Category>();
            var categoryId = categoryToUpdate.Id + 1;
            var controller = new CategoriesController(mockRepository);

            //Act
            var result = await controller.PutCategory(categoryId, categoryToUpdate);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PutCategory_WithNotExistingCategory_ReturnsNotFound()
        {
            //Arrage
            var categoryToUpdate = A.Dummy<Category>();
            A.CallTo(() => mockRepository.IsExist(categoryToUpdate.Id))
                .Returns(false);
            var categoryId = categoryToUpdate.Id;
            var controller = new CategoriesController(mockRepository);

            //Act
            var result = await controller.PutCategory(categoryId, categoryToUpdate);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task PutCategory_WithExistingCategory_ReturnsNoContent()
        {
            //Arrage
            var categoryToUpdate = A.Dummy<Category>();
            A.CallTo(() => mockRepository.IsExist(categoryToUpdate.Id))
                .Returns(true);
            var categoryId = categoryToUpdate.Id;
            var controller = new CategoriesController(mockRepository);

            //Act
            var result = await controller.PutCategory(categoryId, categoryToUpdate);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task PostCategory_WithCategoryToCreate_ReturnsCreatedCategory()
        {
            //Arrage
            var categoryToCreate = A.Dummy<PostCategoryDto>();
            var controller = new CategoriesController(mockRepository);

            //Act
            var result = await controller.PostCategory(categoryToCreate);

            //Assert
            var createdCategory = (result.Result as CreatedAtActionResult).Value as Category;
            result.Result.Should().BeEquivalentTo(
                createdCategory,
                Options => Options.ComparingByMembers<Category>().ExcludingMissingMembers()
                );
        }

        [Fact]
        public async Task DeleteCategory_WithNotExistingCategory_ReturnsNotFound()
        {
            //Arrage
            var providedCategoryId = 1;
            A.CallTo(() => mockRepository.IsExist(providedCategoryId))
                .Returns(false);
            var controller = new CategoriesController(mockRepository);

            //Act
            var result = await controller.DeleteCategory(providedCategoryId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteCategory_WithExistingCategory_ReturnsNoContent()
        {
            //Arrage
            var existingCategory = A.Dummy<Category>();
            A.CallTo(() => mockRepository.IsExist(existingCategory.Id))
                .Returns(true);
            var controller = new CategoriesController(mockRepository);

            //Act
            var result = await controller.DeleteCategory(existingCategory.Id);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
