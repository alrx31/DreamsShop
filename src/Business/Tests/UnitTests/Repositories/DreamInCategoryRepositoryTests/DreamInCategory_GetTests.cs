using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.DreamInCategoryRepositoryTests;

public class DreamInCategory_GetTests : BaseRepositoryTest
{
    private readonly IDreamInCategoryRepository _dreamInCategoryRepository;
    
    public DreamInCategory_GetTests()
    {
        _dreamInCategoryRepository = new DreamInCategoryRepository(Context);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnDreamInCategory()
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
        var result = await _dreamInCategoryRepository.GetAsync(dreamInCategory.Id);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(dreamInCategory);
    }
}