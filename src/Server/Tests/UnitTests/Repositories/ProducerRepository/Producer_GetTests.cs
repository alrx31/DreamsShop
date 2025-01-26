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
        var faker = new Faker();
        
        var producer = new Producer
        {
            Id = faker.Random.Guid(),
            Name = faker.Company.CompanyName(),
            Description = faker.Company.CompanyName(),
        };
        
        await Context.Producer.AddAsync(producer);
        await Context.SaveChangesAsync();
        
        // Act
        
        var result = await _producerRepository.GetAsync(producer.Id);
        
        // Arrange
        
        result.Should().BeEquivalentTo(producer);
    }
}