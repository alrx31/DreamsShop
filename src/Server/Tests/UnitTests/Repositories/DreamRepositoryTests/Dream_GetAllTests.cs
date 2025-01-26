using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.DreamRepositoryTests;

public class Dream_GetAllTests : BaseRepositoryTest
{
    private readonly DreamRepository _dreamRepository;

    public Dream_GetAllTests()
    {
        _dreamRepository = new DreamRepository(Context);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnDreams()
    {
        // Arrange
        var dreams = new Faker<Dream>().Generate(3);
        
        await Context.Dream.AddRangeAsync(dreams);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _dreamRepository.GetAllAsync(1, 10);
        
        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(3);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnCount()
    {
        // Arrange
        var dreams = new Faker<Dream>().Generate(3);
        
        await Context.Dream.AddRangeAsync(dreams);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _dreamRepository.GetAllAsync(1, 10);
        
        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(3);
    }
}