using Bogus;
using Domain.Entity;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.ProducerRepository;

public class Producer_DeleteTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.ProducerRepository _producerRepository;

    public Producer_DeleteTests()
    {
        _producerRepository = new Infrastructure.Persistence.Repositories.ProducerRepository(Context);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteProducer()
    {
        // Arrange
        var producer = new Faker<Producer>().Generate();

        await Context.Producer.AddAsync(producer);
        await Context.SaveChangesAsync();
        
        // Act
        await _producerRepository.DeleteAsync(producer);
        await Context.SaveChangesAsync();

        // Assert
        var result = await Context.Producer.FindAsync(producer.Id);

        result.Should().BeNull();
    }
}