using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.CategoryRepositoryTests;

public class Category_AddTests : BaseRepositoryTest
{
    private readonly CategoryRepository _categoryRepository;

    public Category_AddTests()
    {
        _categoryRepository = new CategoryRepository(Context);
    }
    
    [Fact]
    public async Task AddAsync_ShouldAddCategory()
    {
        // Arrange
        var faker = new Faker();
        
        var category = new Category
        {
            Id = faker.Random.Guid(),
            Title = faker.Commerce.Categories(1)[0]
        };
        
        // Act
        await _categoryRepository.AddAsync(category);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.Category.FirstOrDefaultAsync(x => x.Id == category.Id);
        
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(category);
    }
}