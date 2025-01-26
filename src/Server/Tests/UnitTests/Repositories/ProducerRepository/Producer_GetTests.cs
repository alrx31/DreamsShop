using Bogus;
using Domain.Entity;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.ProducerRepository;

public class Producer_GetTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.ProducerRepository _producerRepository;

    public Producer_GetTests()
    {
        _producerRepository = new Infrastructure.Persistence.Repositories.ProducerRepository(Context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducers()
    {
        // Arrange
        var producer = new Faker<Producer>().Generate();
        
        await Context.Producer.AddAsync(producer);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _producerRepository.GetAsync(producer.Id);
        
        // Arrange
        result.Should().BeEquivalentTo(producer);
    }
}