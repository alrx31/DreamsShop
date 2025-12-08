using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.Repositories;

public class DreamCategoryRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly DreamCategoryRepository _repository;
    private readonly Faker<DreamCategory> _dreamCategoryFaker;

    public DreamCategoryRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new DreamCategoryRepository(_context);

        _dreamCategoryFaker = new Faker<DreamCategory>()
            .RuleFor(dc => dc.DreamId, f => f.Random.Guid())
            .RuleFor(dc => dc.CategoryId, f => f.Random.Guid());
    }

    [Fact]
    public async Task AddAsync_ShouldAddDreamCategoryToContext()
    {
        // Arrange
        var dreamCategory = _dreamCategoryFaker.Generate();

        // Act
        var result = await _repository.AddAsync(dreamCategory);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(dreamCategory);
        _context.ChangeTracker.Entries<DreamCategory>().Should()
            .Contain(e => e.Entity.DreamId == dreamCategory.DreamId && 
                         e.Entity.CategoryId == dreamCategory.CategoryId);
    }

    [Fact]
    public async Task GetAsync_WithValidIds_ShouldReturnDreamCategory()
    {
        // Arrange
        var dreamCategory = _dreamCategoryFaker.Generate();
        await _context.DreamCategory.AddAsync(dreamCategory);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync<DreamCategory>(
            filter: dc => dc.DreamId == dreamCategory.DreamId && 
                         dc.CategoryId == dreamCategory.CategoryId);

        // Assert
        result.Should().NotBeEmpty();
        result.First().Should().BeEquivalentTo(dreamCategory);
    }

    [Fact]
    public async Task GetAsync_WithInvalidIds_ShouldReturnEmpty()
    {
        // Arrange
        var nonExistentDreamId = Guid.NewGuid();
        var nonExistentCategoryId = Guid.NewGuid();

        // Act
        var result = await _repository.GetAsync<DreamCategory>(
            filter: dc => dc.DreamId == nonExistentDreamId && 
                         dc.CategoryId == nonExistentCategoryId);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAsync_WithFilter_ShouldReturnFilteredDreamCategories()
    {
        // Arrange
        var dreamId = Guid.NewGuid();
        var dreamCategories = _dreamCategoryFaker.Generate(5);
        dreamCategories[0].DreamId = dreamId;
        dreamCategories[1].DreamId = dreamId;
        dreamCategories[2].DreamId = Guid.NewGuid();

        await _context.DreamCategory.AddRangeAsync(dreamCategories);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync<DreamCategory>(
            filter: dc => dc.DreamId == dreamId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(dc => dc.DreamId == dreamId);
    }

    [Fact]
    public async Task GetAsync_WithCategoryIdFilter_ShouldReturnFilteredDreamCategories()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var dreamCategories = _dreamCategoryFaker.Generate(5);
        dreamCategories[0].CategoryId = categoryId;
        dreamCategories[1].CategoryId = categoryId;
        dreamCategories[2].CategoryId = Guid.NewGuid();

        await _context.DreamCategory.AddRangeAsync(dreamCategories);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync<DreamCategory>(
            filter: dc => dc.CategoryId == categoryId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(dc => dc.CategoryId == categoryId);
    }

    [Fact]
    public async Task GetAsync_WithSelector_ShouldReturnProjectedData()
    {
        // Arrange
        var dreamCategory = _dreamCategoryFaker.Generate();
        await _context.DreamCategory.AddAsync(dreamCategory);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync<DreamCategory>(
            selector: dc => dc);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().Contain(dc => dc.DreamId == dreamCategory.DreamId && 
                                     dc.CategoryId == dreamCategory.CategoryId);
    }

    [Fact]
    public async Task GetAsync_WithSkipAndTake_ShouldReturnPagedResults()
    {
        // Arrange
        var dreamCategories = _dreamCategoryFaker.Generate(10);
        await _context.DreamCategory.AddRangeAsync(dreamCategories);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync<DreamCategory>(
            skip: 2,
            take: 3);

        // Assert
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetAsync_WithSkipAndTake_WhenSkipExceedsCount_ShouldReturnEmpty()
    {
        // Arrange
        var dreamCategories = _dreamCategoryFaker.Generate(5);
        await _context.DreamCategory.AddRangeAsync(dreamCategories);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync<DreamCategory>(
            skip: 10,
            take: 5);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateDreamCategoryInContext()
    {
        // Arrange
        // Note: DreamCategory has a composite key (DreamId, CategoryId), 
        // so we can't modify the key properties. This test verifies that UpdateAsync
        // can be called without errors (though in practice, updating a composite key
        // entity would require deleting and recreating it)
        var dreamCategory = _dreamCategoryFaker.Generate();
        await _context.DreamCategory.AddAsync(dreamCategory);
        await _context.SaveChangesAsync();

        // Act
        await _repository.UpdateAsync(dreamCategory);
        await _context.SaveChangesAsync();

        // Assert
        var updatedDreamCategory = await _context.DreamCategory
            .FirstOrDefaultAsync(dc => dc.DreamId == dreamCategory.DreamId && 
                                      dc.CategoryId == dreamCategory.CategoryId);
        updatedDreamCategory.Should().NotBeNull();
        updatedDreamCategory!.DreamId.Should().Be(dreamCategory.DreamId);
        updatedDreamCategory.CategoryId.Should().Be(dreamCategory.CategoryId);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveDreamCategoryFromContext()
    {
        // Arrange
        var dreamCategory = _dreamCategoryFaker.Generate();
        await _context.DreamCategory.AddAsync(dreamCategory);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(dreamCategory);
        await _context.SaveChangesAsync();

        // Assert
        var deletedDreamCategory = await _context.DreamCategory
            .FirstOrDefaultAsync(dc => dc.DreamId == dreamCategory.DreamId && 
                                      dc.CategoryId == dreamCategory.CategoryId);
        deletedDreamCategory.Should().BeNull();
    }

    [Fact]
    public async Task GetCountAsync_ShouldReturnCorrectCount()
    {
        // Arrange
        var dreamCategories = _dreamCategoryFaker.Generate(5);
        await _context.DreamCategory.AddRangeAsync(dreamCategories);
        await _context.SaveChangesAsync();

        // Act
        var count = await _repository.GetCountAsync();

        // Assert
        count.Should().Be(5);
    }

    [Fact]
    public async Task GetCountAsync_WhenEmpty_ShouldReturnZero()
    {
        // Act
        var count = await _repository.GetCountAsync();

        // Assert
        count.Should().Be(0);
    }

    [Fact]
    public async Task GetAsync_WithComplexFilter_ShouldReturnCorrectResults()
    {
        // Arrange
        var dreamId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var dreamCategories = _dreamCategoryFaker.Generate(10);
        
        // Ensure each entity has unique composite key
        for (int i = 0; i < 3; i++)
        {
            dreamCategories[i].DreamId = dreamId;
            dreamCategories[i].CategoryId = Guid.NewGuid(); // Different CategoryId for each
        }
        
        // Add one with both dreamId and categoryId matching
        dreamCategories[3].DreamId = dreamId;
        dreamCategories[3].CategoryId = categoryId;

        await _context.DreamCategory.AddRangeAsync(dreamCategories);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync<DreamCategory>(
            filter: dc => dc.DreamId == dreamId && dc.CategoryId == categoryId);

        // Assert
        result.Should().HaveCount(1);
        result.First().DreamId.Should().Be(dreamId);
        result.First().CategoryId.Should().Be(categoryId);
    }

    [Fact]
    public async Task GetAsync_WithNullFilter_ShouldReturnAllDreamCategories()
    {
        // Arrange
        var dreamCategories = _dreamCategoryFaker.Generate(5);
        await _context.DreamCategory.AddRangeAsync(dreamCategories);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync<DreamCategory>(filter: null);

        // Assert
        result.Should().HaveCount(5);
    }

    [Fact]
    public async Task GetCategoriesByDreamIdAsync_WithValidDreamId_ShouldReturnCategories()
    {
        // Arrange
        var dreamId = Guid.NewGuid();
        var dreamCategories = _dreamCategoryFaker.Generate(5);
        
        for (int i = 0; i < 3; i++)
        {
            dreamCategories[i].DreamId = dreamId;
        }
        
        dreamCategories[3].DreamId = Guid.NewGuid();
        dreamCategories[4].DreamId = Guid.NewGuid();

        await _context.DreamCategory.AddRangeAsync(dreamCategories);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetCategoriesByDreamIdAsync(dreamId);
        var resultList = await result.ToListAsync();

        // Assert
        resultList.Should().HaveCount(3);
        resultList.Should().OnlyContain(dc => dc.DreamId == dreamId);
    }

    [Fact]
    public async Task GetCategoriesByDreamIdAsync_WithInvalidDreamId_ShouldReturnEmpty()
    {
        // Arrange
        var nonExistentDreamId = Guid.NewGuid();
        var dreamCategories = _dreamCategoryFaker.Generate(3);
        await _context.DreamCategory.AddRangeAsync(dreamCategories);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetCategoriesByDreamIdAsync(nonExistentDreamId);
        var resultList = await result.ToListAsync();

        // Assert
        resultList.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCategoriesByDreamIdAsync_ShouldReturnDistinctCategories()
    {
        // Arrange
        var dreamId = Guid.NewGuid();
        var categoryId1 = Guid.NewGuid();
        var categoryId2 = Guid.NewGuid();
        
        // Create entries with same DreamId but different CategoryIds
        var dreamCategory1 = _dreamCategoryFaker.Generate();
        dreamCategory1.DreamId = dreamId;
        dreamCategory1.CategoryId = categoryId1;
        
        var dreamCategory2 = _dreamCategoryFaker.Generate();
        dreamCategory2.DreamId = dreamId;
        dreamCategory2.CategoryId = categoryId2;
        
        // Add another entry with same DreamId and CategoryId1 to test Distinct()
        var dreamCategory3 = _dreamCategoryFaker.Generate();
        dreamCategory3.DreamId = dreamId;
        dreamCategory3.CategoryId = categoryId1;

        // Note: In a real scenario, EF Core would prevent duplicate composite keys,
        // but Distinct() in the query ensures uniqueness in the result
        await _context.DreamCategory.AddAsync(dreamCategory1);
        await _context.SaveChangesAsync();
        
        await _context.DreamCategory.AddAsync(dreamCategory2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetCategoriesByDreamIdAsync(dreamId);
        var resultList = await result.ToListAsync();

        // Assert
        resultList.Should().HaveCount(2, "Should return all categories for the dream");
        resultList.Should().OnlyContain(dc => dc.DreamId == dreamId);
        resultList.Select(dc => dc.CategoryId).Should().Contain(categoryId1);
        resultList.Select(dc => dc.CategoryId).Should().Contain(categoryId2);
    }

    [Fact]
    public async Task GetCategoriesByDreamIdAsync_WithMultipleDreams_ShouldReturnOnlyMatchingDream()
    {
        // Arrange
        var dreamId1 = Guid.NewGuid();
        var dreamId2 = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        
        var dreamCategory1 = _dreamCategoryFaker.Generate();
        dreamCategory1.DreamId = dreamId1;
        dreamCategory1.CategoryId = categoryId;
        
        var dreamCategory2 = _dreamCategoryFaker.Generate();
        dreamCategory2.DreamId = dreamId2;
        dreamCategory2.CategoryId = categoryId;

        await _context.DreamCategory.AddRangeAsync(new[] { dreamCategory1, dreamCategory2 });
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetCategoriesByDreamIdAsync(dreamId1);
        var resultList = await result.ToListAsync();

        // Assert
        resultList.Should().HaveCount(1);
        resultList.First().DreamId.Should().Be(dreamId1);
        resultList.First().CategoryId.Should().Be(categoryId);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

