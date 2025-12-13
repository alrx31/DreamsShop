using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.IntegrationTests.Repositories;

public class CategoryRepositoryTests : BaseRepositoryTest
{
    private readonly CategoryRepository _repository;

    public CategoryRepositoryTests()
    {
        _repository = new CategoryRepository(Context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddCategory()
    {
        // Arrange
        var faker = new Faker();
        var category = new Category
        {
            CategoryId = faker.Random.Guid(),
            Title = faker.Commerce.Categories(1)[0],
            Description = faker.Lorem.Sentence()
        };

        // Act
        var addedCategory = await _repository.AddAsync(category);
        await Context.SaveChangesAsync();

        // Assert
        var result = await Context.Category.FindAsync(addedCategory.CategoryId);
        result.Should().Be(category);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnCategory()
    {
        // Arrange
        var faker = new Faker();
        var category = new Category
        {
            CategoryId = faker.Random.Guid(),
            Title = faker.Commerce.Categories(1)[0],
            Description = faker.Lorem.Sentence()
        };
        await Context.Category.AddAsync(category);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync(new[] { category.CategoryId });

        // Assert
        result.Should().Be(category);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCategory()
    {
        // Arrange
        var faker = new Faker();
        var category = new Category
        {
            CategoryId = faker.Random.Guid(),
            Title = faker.Commerce.Categories(1)[0],
            Description = faker.Lorem.Sentence()
        };
        await Context.Category.AddAsync(category);
        await Context.SaveChangesAsync();

        var newTitle = faker.Commerce.Categories(1)[0];
        var newDescription = faker.Lorem.Sentence();
        
        category.Title = newTitle;
        category.Description = newDescription;

        // Act
        await _repository.UpdateAsync(category);
        await Context.SaveChangesAsync();

        // Assert
        var result = await Context.Category.FindAsync(category.CategoryId);
        
        result.Should().NotBeNull();
        result!.Title.Should().Be(newTitle);
        result.Description.Should().Be(newDescription);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteCategory()
    {
        // Arrange
        var faker = new Faker();
        var category = new Category
        {
            CategoryId = faker.Random.Guid(),
            Title = faker.Commerce.Categories(1)[0],
            Description = faker.Lorem.Sentence()
        };
        await Context.Category.AddAsync(category);
        await Context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(category);
        await Context.SaveChangesAsync();

        // Assert
        var result = await Context.Category.FindAsync(category.CategoryId);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAsync_ShouldReturnAllCategories()
    {
        // Arrange
        var faker = new Faker();
        var categories = new List<Category>
        {
            new() { CategoryId = faker.Random.Guid(), Title = faker.Commerce.Categories(1)[0], Description = faker.Lorem.Sentence() },
            new() { CategoryId = faker.Random.Guid(), Title = faker.Commerce.Categories(1)[0], Description = faker.Lorem.Sentence() },
            new() { CategoryId = faker.Random.Guid(), Title = faker.Commerce.Categories(1)[0], Description = faker.Lorem.Sentence() }
        };
        await Context.Category.AddRangeAsync(categories);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync<Category>();

        // Assert
        result.Should().BeEquivalentTo(categories.AsQueryable());
    }
}
