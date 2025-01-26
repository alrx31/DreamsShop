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
        var dreamInCategory = new Faker<DreamInCategory>().Generate();
        
        await Context.DreamInCategory.AddAsync(dreamInCategory);
        await Context.SaveChangesAsync();
        
        dreamInCategory.CategoryId = faker.Random.Guid();
        dreamInCategory.DreamId = faker.Random.Guid();
        
        // Act
        await _dreamInCategoryRepository.UpdateAsync(dreamInCategory);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.DreamInCategory.FirstOrDefaultAsync(x => x.Id == dreamInCategory.Id);
        
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(dreamInCategory);
    }
}