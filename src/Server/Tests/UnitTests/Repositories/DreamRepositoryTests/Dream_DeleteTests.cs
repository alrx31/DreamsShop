using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.DreamRepositoryTests;

public class Dream_DeleteTests : BaseRepositoryTest
{
    private readonly DreamRepository _dreamRepository;

    public Dream_DeleteTests()
    {
        _dreamRepository = new DreamRepository(Context);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldDeleteDream()
    {
        // Arrange
        var dream = new Faker<Dream>().Generate();
        
        await Context.Dream.AddAsync(dream);
        await Context.SaveChangesAsync();
        
        // Act
        await _dreamRepository.DeleteAsync(dream);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.Dream.FindAsync(dream.Id);
        
        result.Should().BeNull();
    }
}