using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.IntegrationTests.Repositories;

public class DreamCategoryRepositoryTests : BaseRepositoryTest
{
    private readonly DreamCategoryRepository _repository;
    private readonly DreamRepository _dreamRepository;
    private readonly CategoryRepository _categoryRepository;

    public DreamCategoryRepositoryTests()
    {
        _repository = new DreamCategoryRepository(Context);
        _dreamRepository = new DreamRepository(Context);
        _categoryRepository = new CategoryRepository(Context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddDreamCategory()
    {
        // Arrange
        var faker = new Faker();
        var dream = new Dream { DreamId = faker.Random.Guid(), Title = faker.Lorem.Sentence(), Description = faker.Lorem.Paragraph(), ImageFileName = "test.jpg" };
        var category = new Category { CategoryId = faker.Random.Guid(), Title = faker.Commerce.Categories(1)[0] };
        await _dreamRepository.AddAsync(dream);
        await _categoryRepository.AddAsync(category);
        await Context.SaveChangesAsync();
        
        var dreamCategory = new DreamCategory
        {
            DreamId = dream.DreamId,
            CategoryId = category.CategoryId
        };

        // Act
        await _repository.AddAsync(dreamCategory);
        await Context.SaveChangesAsync();

        // Assert
        var result = await Context.DreamCategory.FindAsync(dream.DreamId, category.CategoryId);
        result.Should().Be(dreamCategory);
    }

    [Fact]
    public async Task GetCategoriesByDreamIdAsync_ShouldReturnCategories()
    {
        // Arrange
        var faker = new Faker();
        var dream = new Dream { DreamId = faker.Random.Guid(), Title = faker.Lorem.Sentence(), Description = faker.Lorem.Paragraph(), ImageFileName = "test.jpg" };
        var category1 = new Category { CategoryId = faker.Random.Guid(), Title = faker.Commerce.Categories(1)[0] };
        var category2 = new Category { CategoryId = faker.Random.Guid(), Title = faker.Commerce.Categories(1)[0] };
        await _dreamRepository.AddAsync(dream);
        await _categoryRepository.AddAsync(category1);
        await _categoryRepository.AddAsync(category2);
        await Context.SaveChangesAsync();
        
        var dreamCategory1 = new DreamCategory { DreamId = dream.DreamId, CategoryId = category1.CategoryId };
        var dreamCategory2 = new DreamCategory { DreamId = dream.DreamId, CategoryId = category2.CategoryId };
        await _repository.AddAsync(dreamCategory1);
        await _repository.AddAsync(dreamCategory2);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetCategoriesByDreamIdAsync(dream.DreamId);

        // Assert
        result.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldDeleteDreamCategory()
    {
        // Arrange
        var faker = new Faker();
        var dream = new Dream { DreamId = faker.Random.Guid(), Title = faker.Lorem.Sentence(), Description = faker.Lorem.Paragraph(), ImageFileName = "test.jpg" };
        var category = new Category { CategoryId = faker.Random.Guid(), Title = faker.Commerce.Categories(1)[0] };
        await _dreamRepository.AddAsync(dream);
        await _categoryRepository.AddAsync(category);
        await Context.SaveChangesAsync();
        
        var dreamCategory = new DreamCategory { DreamId = dream.DreamId, CategoryId = category.CategoryId };
        await _repository.AddAsync(dreamCategory);
        await Context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(dreamCategory);
        await Context.SaveChangesAsync();

        // Assert
        var result = await Context.DreamCategory.FindAsync(dream.DreamId, category.CategoryId);
        result.Should().BeNull();
    }
}
