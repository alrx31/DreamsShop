using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.CategoryRepositoryTests;

public class Category_GetAllTests : BaseRepositoryTest
{
    private readonly CategoryRepository _categoryRepository;

    public Category_GetAllTests()
    {
        _categoryRepository = new CategoryRepository(Context);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCategories_WhenPageAndPageSizeProvided()
    {
        // Arrange
        var faker = new Faker();
        
        var categories = new List<Category>
        {
            new Category
            {
                Id = faker.Random.Guid(),
                Title = faker.Commerce.Categories(1)[0]
            },
            new Category
            {
                Id = faker.Random.Guid(),
                Title = faker.Commerce.Categories(1)[0]
            },
            new Category
            {
                Id = faker.Random.Guid(),
                Title = faker.Commerce.Categories(1)[0]
            }
        };
        
        await Context.Category.AddRangeAsync(categories);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _categoryRepository.GetAllAsync(0, 2);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(categories.Take(2));
    }
    
    [Fact]
    public async Task GetRangeAsync_ShouldReturnAllCategories_WhenPageAndPageSizeProvided()
    {
        // Arrange
        var faker = new Faker();
        
        var categories = new List<Category>
        {
            new Category
            {
                Id = faker.Random.Guid(),
                Title = faker.Commerce.Categories(1)[0]
            },
            new Category
            {
                Id = faker.Random.Guid(),
                Title = faker.Commerce.Categories(1)[0]
            },
            new Category
            {
                Id = faker.Random.Guid(),
                Title = faker.Commerce.Categories(1)[0]
            }
        };
        
        await Context.Category.AddRangeAsync(categories);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _categoryRepository.GetRangeAsync(0, 2);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(categories.Take(2));
    }
    
    [Fact]
    public async Task GetCountAsync_ShouldReturnCategoriesCount()
    {
        // Arrange
        var faker = new Faker();
        
        var categories = new List<Category>
        {
            new Category
            {
                Id = faker.Random.Guid(),
                Title = faker.Commerce.Categories(1)[0]
            },
            new Category
            {
                Id = faker.Random.Guid(),
                Title = faker.Commerce.Categories(1)[0]
            },
            new Category
            {
                Id = faker.Random.Guid(),
                Title = faker.Commerce.Categories(1)[0]
            }
        };
        
        await Context.Category.AddRangeAsync(categories);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _categoryRepository.GetCountAsync();
        
        // Assert
        result.Should().Be(categories.Count);
    }
}