using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.CategoryRepositoryTests;

public class Category_UpdateTests : BaseRepositoryTest
{
    private readonly CategoryRepository _categoryRepository;

    public Category_UpdateTests()
    {
        _categoryRepository = new CategoryRepository(Context);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldUpdateCategory()
    {
        // Arrange
        var faker = new Faker();
        var category = new Faker<Category>().Generate();
        
        await Context.Category.AddAsync(category);
        await Context.SaveChangesAsync();
        
        // Act
        category.Title = faker.Commerce.Categories(1)[0];
        
        await _categoryRepository.UpdateAsync(category);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.Category.FirstOrDefaultAsync(x => x.Id == category.Id);
        
        result.Should().BeEquivalentTo(category);
    }
}