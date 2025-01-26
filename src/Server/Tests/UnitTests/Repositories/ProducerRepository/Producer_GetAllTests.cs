using Bogus;
using Domain.Entity;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.ProducerRepository;

public class Producer_GetAllTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.ProducerRepository _producerRepository;

    public Producer_GetAllTests()
    {
        _producerRepository = new Infrastructure.Persistence.Repositories.ProducerRepository(Context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducers()
    {
        // Arrange
        var producers = new Faker<Producer>().Generate(2);
        
        await Context.Producer.AddRangeAsync(producers);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _producerRepository.GetAllAsync(1,2);
        
        // Assert
        result.Should().BeEquivalentTo(producers);
    }
}