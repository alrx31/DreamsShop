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
        var faker = new Faker();
        
        var producers = new List<Producer>()
        {
            new Producer()
            {
                Id = faker.Random.Guid(),
                Name = faker.Name.FirstName(),
                Description = faker.Lorem.Paragraph(),
                Rating = faker.Random.Decimal(),
            },
            new Producer()
            {
                Id = faker.Random.Guid(),
                Name = faker.Name.FirstName(),
                Description = faker.Lorem.Paragraph(),
                Rating = faker.Random.Decimal(),
            }
        };
        
        await Context.Producer.AddRangeAsync(producers);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _producerRepository.GetAllAsync(1,2);
        
        // Assert
        result.Should().BeEquivalentTo(producers);
    }
}