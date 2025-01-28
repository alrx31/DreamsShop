using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.DreamInCategoryRepositoryTests;

public class DreamInCategory_GetAllTests : BaseRepositoryTest
{
    private readonly IDreamInCategoryRepository _dreamInCategoryRepository;
    
    public DreamInCategory_GetAllTests()
    {
        _dreamInCategoryRepository = new DreamInCategoryRepository(Context);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnDreamInCategory()
    {
        // Arrange
        var faker = new Faker();
        
        var dreamInCategory = new DreamInCategory
        {
            Id = faker.Random.Guid(),
            DreamId = faker.Random.Guid(),
            CategoryId = faker.Random.Guid()
        };
        
        await Context.DreamInCategory.AddAsync(dreamInCategory);
        
        await Context.SaveChangesAsync();
        
        // Act
        
        var result = await _dreamInCategoryRepository.GetAllAsync(1, 1);
        
        // Assert
        
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Should().BeEquivalentTo(dreamInCategory);
    }
    
    [Fact]
    public async Task GetCountAsync_ShouldReturnCount()
    {
        // Arrange
        var faker = new Faker();
        
        var dreamInCategory = new DreamInCategory
        {
            Id = faker.Random.Guid(),
            DreamId = faker.Random.Guid(),
            CategoryId = faker.Random.Guid()
        };
        
        await Context.DreamInCategory.AddAsync(dreamInCategory);
        
        await Context.SaveChangesAsync();
        
        // Act
        
        var result = await _dreamInCategoryRepository.GetCountAsync();
        
        // Assert
        
        result.Should().Be(1);
    }
}