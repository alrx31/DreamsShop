using Application.UseCases.Category.CategoryCreate;
using Application.DTO;
using AutoMapper;
using Bogus;
using Domain.IRepositories;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.Category;

public class CategoryAddCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CategoryAddCommandHandler _handler;

    public CategoryAddCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _mapperMock = new Mock<IMapper>();
        
        _unitOfWorkMock.Setup(u => u.CategoryRepository).Returns(_categoryRepositoryMock.Object);
        
        _handler = new CategoryAddCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddCategoryAndSaveChanges()
    {
        // Arrange
        var faker = new Faker();
        var createDto = new CategoryCreateDto
        {
            Title = faker.Commerce.Categories(1)[0],
            Description = faker.Lorem.Sentence()
        };
        
        var command = new CategoryAddCommand(createDto);
        
        var category = new Domain.Entity.Category
        {
            CategoryId = faker.Random.Guid(),
            Title = createDto.Title,
            Description = createDto.Description
        };

        _mapperMock.Setup(m => m.Map<Domain.Entity.Category>(command)).Returns(category);
        _categoryRepositoryMock.Setup(r => r.AddAsync(category, CancellationToken.None)).ReturnsAsync(category.CategoryId);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(CancellationToken.None)).Returns(Task.FromResult(1));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(category.CategoryId);
        _mapperMock.Verify(m => m.Map<Domain.Entity.Category>(command), Times.Once);
        _categoryRepositoryMock.Verify(r => r.AddAsync(category, CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}
