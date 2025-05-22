using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.DreamInCategoryRepositoryTests;

public class DreamInCategory_UpdateTests : BaseRepositoryTest
{
    private readonly IDreamInCategoryRepository _dreamInCategoryRepository;
    
    public DreamInCategory_UpdateTests()
    {
        _dreamInCategoryRepository = new DreamInCategoryRepository(Context);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldUpdateDreamInCategory()
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
        
        var updatedDreamInCategory = new DreamInCategory
        {
            Id = dreamInCategory.Id,
            DreamId = faker.Random.Guid(),
            CategoryId = faker.Random.Guid()
        };
        
        dreamInCategory.CategoryId = updatedDreamInCategory.CategoryId;
        dreamInCategory.DreamId = updatedDreamInCategory.DreamId;
        
        // Act
        await _dreamInCategoryRepository.UpdateAsync(dreamInCategory);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.DreamInCategory.FirstOrDefaultAsync(x => x.Id == dreamInCategory.Id);
        
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(updatedDreamInCategory);
    }
}