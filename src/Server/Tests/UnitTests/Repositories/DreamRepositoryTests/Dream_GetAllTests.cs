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
        var faker = new Faker();
        
        var dreams = new List<Dream>
        {
            new()
            {
                Id = faker.Random.Guid(),
                Title = faker.Lorem.Sentence(),
                Desctiption = faker.Lorem.Paragraph(),
                ImageMediaId = faker.Random.Guid(),
                PreviewMediaId = faker.Random.Guid(),
                ProducerId = faker.Random.Guid(),
            },
            new()
            {
                Id = faker.Random.Guid(),
                Title = faker.Lorem.Sentence(),
                Desctiption = faker.Lorem.Paragraph(),
                ImageMediaId = faker.Random.Guid(),
                PreviewMediaId = faker.Random.Guid(),
                ProducerId = faker.Random.Guid(),
            },
            new()
            {
                Id = faker.Random.Guid(),
                Title = faker.Lorem.Sentence(),
                Desctiption = faker.Lorem.Paragraph(),
                ImageMediaId = faker.Random.Guid(),
                PreviewMediaId = faker.Random.Guid(),
                ProducerId = faker.Random.Guid(),
            }
        };
        
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
        var faker = new Faker();
        
        var dreams = new List<Dream>
        {
            new()
            {
                Id = faker.Random.Guid(),
                Title = faker.Lorem.Sentence(),
                Desctiption = faker.Lorem.Paragraph(),
                ImageMediaId = faker.Random.Guid(),
                PreviewMediaId = faker.Random.Guid(),
                ProducerId = faker.Random.Guid(),
            },
            new()
            {
                Id = faker.Random.Guid(),
                Title = faker.Lorem.Sentence(),
                Desctiption = faker.Lorem.Paragraph(),
                ImageMediaId = faker.Random.Guid(),
                PreviewMediaId = faker.Random.Guid(),
                ProducerId = faker.Random.Guid(),
            },
            new()
            {
                Id = faker.Random.Guid(),
                Title = faker.Lorem.Sentence(),
                Desctiption = faker.Lorem.Paragraph(),
                ImageMediaId = faker.Random.Guid(),
                PreviewMediaId = faker.Random.Guid(),
                ProducerId = faker.Random.Guid(),
            }
        };
        
        await Context.Dream.AddRangeAsync(dreams);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _dreamRepository.GetAllAsync(1, 10);
        
        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(3);
    }
}