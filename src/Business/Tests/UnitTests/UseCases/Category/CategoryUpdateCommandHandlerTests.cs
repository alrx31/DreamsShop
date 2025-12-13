using Application.UseCases.Category.CategoryUpdate;
using Application.DTO;
using Application.Exceptions;
using Bogus;
using Domain.IRepositories;
using FluentAssertions;
using Moq;
using Tests.TestHelpers;

namespace Tests.UnitTests.UseCases.Category;

public class CategoryUpdateCommandHandlerTests : IDisposable
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly CategoryUpdateCommandHandler _handler;
    private readonly ServiceLocatorTestHelper.ServiceLocatorTestScope _serviceScope;

    public CategoryUpdateCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        
        _unitOfWorkMock.Setup(u => u.CategoryRepository).Returns(_categoryRepositoryMock.Object);

        _serviceScope = ServiceLocatorTestHelper.UseServiceLocator(
            (typeof(IUnitOfWork), _unitOfWorkMock.Object));
        
        _handler = new CategoryUpdateCommandHandler();
    }

    [Fact]
    public async Task Handle_ShouldUpdateCategory()
    {
        // Arrange
        var faker = new Faker();
        var categoryId = faker.Random.Guid();
        var updateDto = new CategoryUpdateDto
        {
            Title = faker.Commerce.Categories(1)[0],
            Description = faker.Lorem.Sentence()
        };
        var command = new CategoryUpdateCommand(updateDto, categoryId);
        
        var category = new Domain.Entity.Category
        {
            CategoryId = categoryId,
            Title = faker.Commerce.Categories(1)[0],
            Description = faker.Lorem.Sentence()
        };

        _categoryRepositoryMock.Setup(r => r.GetAsync(new [] {categoryId}, CancellationToken.None)).ReturnsAsync(category);
        _categoryRepositoryMock.Setup(r => r.UpdateAsync(category, CancellationToken.None)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(CancellationToken.None)).Returns(Task.FromResult(1));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        category.Title.Should().Be(updateDto.Title);
        category.Description.Should().Be(updateDto.Description);
        _categoryRepositoryMock.Verify(r => r.GetAsync(new [] {categoryId}, CancellationToken.None), Times.Once);
        _categoryRepositoryMock.Verify(r => r.UpdateAsync(category, CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var updateDto = new CategoryUpdateDto
        {
            Title = new Faker().Commerce.Categories(1)[0],
            Description = new Faker().Lorem.Sentence()
        };
        var command = new CategoryUpdateCommand(updateDto, categoryId);

        _categoryRepositoryMock.Setup(r => r.GetAsync(new [] {categoryId}, CancellationToken.None)).ReturnsAsync((Domain.Entity.Category)null!);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _categoryRepositoryMock.Verify(r => r.GetAsync(new [] {categoryId}, CancellationToken.None), Times.Once);
        _categoryRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entity.Category>(), CancellationToken.None), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Never);
    }

    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}
