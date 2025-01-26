using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.DreamRepositoryTests;

public class Dream_UpdateTests : BaseRepositoryTest
{
    private readonly DreamRepository _dreamRepository;

    public Dream_UpdateTests()
    {
        _dreamRepository = new DreamRepository(Context);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldUpdateDream()
    {
        // Arrange
        var faker = new Faker();
        var dream = new Faker<Dream>().Generate();
        
        await Context.Dream.AddAsync(dream);
        await Context.SaveChangesAsync();
        
        dream.Title = faker.Random.Word();
        dream.Desctiption = faker.Random.Word();
        dream.ImageMediaId = faker.Random.Guid();
        dream.PreviewMediaId = faker.Random.Guid();
        dream.ProducerId = faker.Random.Guid();
        
        // Act
        await _dreamRepository.UpdateAsync(dream);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.Dream.FindAsync(dream.Id);
        
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(dream);
    }
}