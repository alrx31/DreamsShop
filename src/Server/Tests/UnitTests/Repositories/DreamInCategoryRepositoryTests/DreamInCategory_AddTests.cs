using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.DreamInCategoryRepositoryTests;

public class DreamInCategory_AddTests : BaseRepositoryTest
{
    private readonly IDreamInCategoryRepository _dreamInCategoryRepository;
    
    public DreamInCategory_AddTests()
    {
        _dreamInCategoryRepository = new DreamInCategoryRepository(Context);
    }
    
    [Fact]
    public async Task AddAsync_ShouldAddDreamInCategory()
    {
        // Arrange
        var faker = new Faker();
        
        var dreamInCategory = new DreamInCategory
        {
            Id = faker.Random.Guid(),
            Dream_Id = faker.Random.Guid(),
            Category_Id = faker.Random.Guid()
        };
        
        // Act
        
        await _dreamInCategoryRepository.AddAsync(dreamInCategory);
        
        await Context.SaveChangesAsync();
        
        // Assert
        
        var result = await Context.DreamInCategory.FirstOrDefaultAsync(x => x.Id == dreamInCategory.Id);
        
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(dreamInCategory);
    }
}