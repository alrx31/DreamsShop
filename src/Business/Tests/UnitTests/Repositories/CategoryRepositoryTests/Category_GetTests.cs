using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.CategoryRepositoryTests;

public class Category_GetTests : BaseRepositoryTest
{
    private readonly CategoryRepository _categoryRepository;

    public Category_GetTests()
    {
        _categoryRepository = new CategoryRepository(Context);
    }
    
    [Fact]
    public async Task GetAsync_ShouldReturnCategory()
    {
        // Arrange
        var faker = new Faker();
        
        var category = new Category
        {
            Id = faker.Random.Guid(),
            Title = faker.Commerce.Categories(1)[0]
        };
        
        await Context.Category.AddAsync(category);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _categoryRepository.GetAsync(category.Id);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(category);
    }
    
    [Fact]
    public async Task GetAsync_ShouldReturnNull()
    {
        // Arrange
        var faker = new Faker();
        
        var category = new Category
        {
            Id = faker.Random.Guid(),
            Title = faker.Commerce.Categories(1)[0]
        };
        
        // Act
        var result = await _categoryRepository.GetAsync(category.Id);
        
        // Assert
        result.Should().BeNull();
    }
    
}