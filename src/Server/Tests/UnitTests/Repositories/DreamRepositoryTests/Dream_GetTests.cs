using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.DreamRepositoryTests;

public class Dream_GetTests : BaseRepositoryTest
{
    private readonly DreamRepository _dreamRepository;

    public Dream_GetTests()
    {
        _dreamRepository = new DreamRepository(Context);
    }
    
    [Fact]
    public async Task GetAsync_ShouldReturnDream()
    {
        // Arrange
        var faker = new Faker();
        
        var dream = new Dream
        {
            Id = faker.Random.Guid(),
            Title = faker.Lorem.Sentence(),
            Desctiption = faker.Lorem.Paragraph(),
            ImageMediaId = faker.Random.Guid(),
            PreviewMediaId = faker.Random.Guid(),
            ProducerId = faker.Random.Guid(),
        };
        
        await Context.Dream.AddAsync(dream);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _dreamRepository.GetAsync(dream.Id);
        
        // Assert
        
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(dream);
    }
}