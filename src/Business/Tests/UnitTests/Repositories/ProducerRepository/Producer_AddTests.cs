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
        var faker = new Faker();
        
        var producer = new Producer()
        {
            Id = faker.Random.Guid(),
            Name = faker.Name.FirstName(),
            Description = faker.Lorem.Paragraph(),
        };
        
        // Act
        await _producerRepository.AddAsync(producer);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.Producer.FindAsync(producer.Id);

        result.Should().BeEquivalentTo(producer);
    }
}