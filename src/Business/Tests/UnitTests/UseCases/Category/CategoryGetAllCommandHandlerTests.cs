using Application.UseCases.Category.CategoryGetAll;
using Bogus;
using Domain.IRepositories;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.Category;

public class CategoryGetAllCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly CategoryGetAllCommandHandler _handler;

    public CategoryGetAllCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        
        _unitOfWorkMock.Setup(u => u.CategoryRepository).Returns(_categoryRepositoryMock.Object);
        
        _handler = new CategoryGetAllCommandHandler(_unitOfWorkMock.Object);
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

        _categoryRepositoryMock.Setup(r => r.GetAllAsync(CancellationToken.None)).ReturnsAsync(categories.AsQueryable());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(categories);
        _categoryRepositoryMock.Verify(r => r.GetAllAsync(CancellationToken.None), Times.Once);
    }
}
