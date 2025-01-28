using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.CategoryRepositoryTests;

public class Category_DeleteTests: BaseRepositoryTest
{
    private readonly CategoryRepository _categoryRepository;
    
    public Category_DeleteTests()
    {
        _categoryRepository = new CategoryRepository(Context);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldDeleteCategory()
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
        
        await _categoryRepository.DeleteAsync(category);
        
        await Context.SaveChangesAsync();
        
        // Assert
        
        var result = await Context.Category.FirstOrDefaultAsync(x => x.Id == category.Id);
        
        result.Should().BeNull();
    }
}