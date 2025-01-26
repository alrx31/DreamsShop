using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.DreamRepositoryTests;

public class Dream_AddTests : BaseRepositoryTest
{
    private readonly DreamRepository _dreamRepository;

    public Dream_AddTests()
    {
        _dreamRepository = new DreamRepository(Context);
    }
    
    [Fact]
    public async Task AddAsync_ShouldAddDream()
    {
        // Arrange
        var dream = new Faker<Dream>().Generate();
        
        // Act
        await _dreamRepository.AddAsync(dream);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.Dream.FindAsync(dream.Id);
        
        result.Should().BeEquivalentTo(dream);
    }
}