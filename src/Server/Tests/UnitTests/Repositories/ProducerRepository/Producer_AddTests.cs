using Bogus;
using Domain.Entity;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.ProducerRepository;

public class Producer_AddTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.ProducerRepository _producerRepository;

    public Producer_AddTests()
    {
        _producerRepository = new Infrastructure.Persistence.Repositories.ProducerRepository(Context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddProducer()
    {
        // Arrange
        var producer = new Faker<Producer>().Generate();
        
        // Act
        await _producerRepository.AddAsync(producer);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.Producer.FindAsync(producer.Id);

        result.Should().BeEquivalentTo(producer);
    }
}