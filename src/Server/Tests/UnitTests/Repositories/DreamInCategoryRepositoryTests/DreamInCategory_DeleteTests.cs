using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.DreamInCategoryRepositoryTests;

public class DreamInCategory_DeleteTests : BaseRepositoryTest
{
    private readonly IDreamInCategoryRepository _dreamInCategoryRepository;
    
    public DreamInCategory_DeleteTests()
    {
        _dreamInCategoryRepository = new DreamInCategoryRepository(Context);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldDeleteDreamInCategory()
    {
        // Arrange
        var faker = new Faker();
        
        var dreamInCategory = new DreamInCategory
        {
            Id = faker.Random.Guid(),
            Dream_Id = faker.Random.Guid(),
            Category_Id = faker.Random.Guid()
        };
        
        await Context.DreamInCategory.AddAsync(dreamInCategory);
        
        await Context.SaveChangesAsync();
        
        // Act
        
        await _dreamInCategoryRepository.DeleteAsync(dreamInCategory);
        
        await Context.SaveChangesAsync();
        
        // Assert
        
        var result = await Context.DreamInCategory.FirstOrDefaultAsync(x => x.Id == dreamInCategory.Id);
        
        result.Should().BeNull();
    }
}