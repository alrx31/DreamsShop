using Application.UseCases.Category.CategoryGet;
using Application.Exceptions;
using Bogus;
using Domain.IRepositories;
using Domain.IService;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.Category;

public class CategoryGetCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<ICacheService<Guid, Domain.Entity.Category>> _cacheServiceMock;
    private readonly CategoryGetCommandHandler _handler;

    public CategoryGetCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _cacheServiceMock = new Mock<ICacheService<Guid, Domain.Entity.Category>>();
        
        _unitOfWorkMock.Setup(u => u.CategoryRepository).Returns(_categoryRepositoryMock.Object);
        
        _handler = new CategoryGetCommandHandler(_unitOfWorkMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCategoryFromCache_WhenCategoryExistsInCache()
    {
        // Arrange
        var faker = new Faker();
        var categoryId = faker.Random.Guid();
        var command = new CategoryGetCommand(categoryId);
        
        var category = new Domain.Entity.Category
        {
            CategoryId = categoryId,
            Title = faker.Commerce.Categories(1)[0],
            Description = faker.Lorem.Sentence()
        };

        _cacheServiceMock.Setup(c => c.GetAsync(categoryId)).ReturnsAsync(category);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(category);
        _cacheServiceMock.Verify(c => c.GetAsync(categoryId), Times.Once);
        _categoryRepositoryMock.Verify(r => r.GetAsync(It.IsAny<Guid[]>(), CancellationToken.None), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<Guid>(), It.IsAny<Domain.Entity.Category>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnCategoryFromRepositoryAndSetCache_WhenCategoryDoesNotExistInCache()
    {
        // Arrange
        var faker = new Faker();
        var categoryId = faker.Random.Guid();
        var command = new CategoryGetCommand(categoryId);
        
        var category = new Domain.Entity.Category
        {
            CategoryId = categoryId,
            Title = faker.Commerce.Categories(1)[0],
            Description = faker.Lorem.Sentence()
        };

        _cacheServiceMock.Setup(c => c.GetAsync(categoryId)).ReturnsAsync((Domain.Entity.Category)null);
        _categoryRepositoryMock.Setup(r => r.GetAsync(new [] {categoryId}, CancellationToken.None)).ReturnsAsync(category);
        _cacheServiceMock.Setup(c => c.SetAsync(categoryId, category)).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(category);
        _cacheServiceMock.Verify(c => c.GetAsync(categoryId), Times.Once);
        _categoryRepositoryMock.Verify(r => r.GetAsync(new [] {categoryId}, CancellationToken.None), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(categoryId, category), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new CategoryGetCommand(categoryId);

        _cacheServiceMock.Setup(c => c.GetAsync(categoryId)).ReturnsAsync((Domain.Entity.Category)null);
        _categoryRepositoryMock.Setup(r => r.GetAsync(new [] {categoryId}, CancellationToken.None)).ReturnsAsync((Domain.Entity.Category)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _cacheServiceMock.Verify(c => c.GetAsync(categoryId), Times.Once);
        _categoryRepositoryMock.Verify(r => r.GetAsync(new [] {categoryId}, CancellationToken.None), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<Guid>(), It.IsAny<Domain.Entity.Category>()), Times.Never);
    }
}
