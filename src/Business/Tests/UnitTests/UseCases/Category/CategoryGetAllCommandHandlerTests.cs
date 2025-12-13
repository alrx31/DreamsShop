using System;
using System.Linq.Expressions;
using Application.UseCases.Category.CategoryGetAll;
using Bogus;
using Domain.IRepositories;
using FluentAssertions;
using Moq;
using Tests.TestHelpers;

namespace Tests.UnitTests.UseCases.Category;

public class CategoryGetAllCommandHandlerTests : IDisposable
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly CategoryGetAllCommandHandler _handler;
    private readonly ServiceLocatorTestHelper.ServiceLocatorTestScope _serviceScope;

    public CategoryGetAllCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        
        _unitOfWorkMock.Setup(u => u.CategoryRepository).Returns(_categoryRepositoryMock.Object);

        _serviceScope = ServiceLocatorTestHelper.UseServiceLocator(
            (typeof(IUnitOfWork), _unitOfWorkMock.Object));
        
        _handler = new CategoryGetAllCommandHandler();
    }

    [Fact]
    public async Task Handle_ShouldReturnAllCategories()
    {
        // Arrange
        var faker = new Faker();
        var categories = new List<Domain.Entity.Category>
        {
            new() { CategoryId = faker.Random.Guid(), Title = faker.Commerce.Categories(1)[0], Description = faker.Lorem.Sentence() },
            new() { CategoryId = faker.Random.Guid(), Title = faker.Commerce.Categories(1)[0], Description = faker.Lorem.Sentence() },
            new() { CategoryId = faker.Random.Guid(), Title = faker.Commerce.Categories(1)[0], Description = faker.Lorem.Sentence() }
        };
        
        var command = new CategoryGetAllCommand();

        _categoryRepositoryMock.Setup(r => r.GetAsync<Domain.Entity.Category>(
                It.IsAny<Expression<Func<Domain.Entity.Category, bool>>?>(),
                It.IsAny<Expression<Func<Domain.Entity.Category, Domain.Entity.Category>>?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                CancellationToken.None))
            .ReturnsAsync(categories);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(categories);
        _categoryRepositoryMock.Verify(r => r.GetAsync<Domain.Entity.Category>(
            It.IsAny<Expression<Func<Domain.Entity.Category, bool>>?>(),
            It.IsAny<Expression<Func<Domain.Entity.Category, Domain.Entity.Category>>?>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            CancellationToken.None), Times.Once);
    }

    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}
