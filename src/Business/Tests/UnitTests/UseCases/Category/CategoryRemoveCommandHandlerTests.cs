using Application.UseCases.Category.CategoryRemove;
using Application.Exceptions;
using Bogus;
using Domain.IRepositories;
using Domain.IService;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.Category;

public class CategoryRemoveCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<ICacheService<Guid, Domain.Entity.Category>> _cacheServiceMock;
    private readonly CategoryRemoveCommandHandler _handler;

    public CategoryRemoveCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _cacheServiceMock = new Mock<ICacheService<Guid, Domain.Entity.Category>>();
        
        _unitOfWorkMock.Setup(u => u.CategoryRepository).Returns(_categoryRepositoryMock.Object);
        
        _handler = new CategoryRemoveCommandHandler(_unitOfWorkMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemoveCategoryAndClearCache()
    {
        // Arrange
        var faker = new Faker();
        var categoryId = faker.Random.Guid();
        var command = new CategoryRemoveCommand(categoryId);
        
        var category = new Domain.Entity.Category
        {
            CategoryId = categoryId,
            Title = faker.Commerce.Categories(1)[0],
            Description = faker.Lorem.Sentence()
        };

        _categoryRepositoryMock.Setup(r => r.GetAsync(new [] {categoryId}, CancellationToken.None)).ReturnsAsync(category);
        _cacheServiceMock.Setup(c => c.RemoveAsync(categoryId)).Returns(Task.CompletedTask);
        _categoryRepositoryMock.Setup(r => r.DeleteAsync(category, CancellationToken.None)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(CancellationToken.None)).Returns(Task.FromResult(1));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _categoryRepositoryMock.Verify(r => r.GetAsync(new [] {categoryId}, CancellationToken.None), Times.Once);
        _cacheServiceMock.Verify(c => c.RemoveAsync(categoryId), Times.Once);
        _categoryRepositoryMock.Verify(r => r.DeleteAsync(category, CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new CategoryRemoveCommand(categoryId);

        _categoryRepositoryMock.Setup(r => r.GetAsync(new [] {categoryId}, CancellationToken.None)).ReturnsAsync((Domain.Entity.Category)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _categoryRepositoryMock.Verify(r => r.GetAsync(new [] {categoryId}, CancellationToken.None), Times.Once);
        _cacheServiceMock.Verify(c => c.RemoveAsync(It.IsAny<Guid>()), Times.Never);
        _categoryRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Domain.Entity.Category>(), CancellationToken.None), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Never);
    }
}
