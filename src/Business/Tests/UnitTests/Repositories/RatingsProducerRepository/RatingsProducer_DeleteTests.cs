using Bogus;
using Domain.Entity;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.RatingsProducerRepository;

public class RatingsProducer_DeleteTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.RatingsProducerRepository _repository;

    public RatingsProducer_DeleteTests()
    {
        _repository = new Infrastructure.Persistence.Repositories.RatingsProducerRepository(Context);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteRatingsProducer()
    {
        // Arrange
        var faker = new Faker();
        var ratingsProducer = new RatingsProducer
        {
            Id = faker.Random.Guid(),
            ProducerId = faker.Random.Guid(),
            ConsumerId = faker.Random.Guid(),
            Value = faker.Random.Int(),
            CreatedAt = faker.Date.Past()
        };
        
        await Context.AddAsync(ratingsProducer);
        await Context.SaveChangesAsync();
        
        // Act
        await _repository.DeleteAsync(ratingsProducer);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.RatingsProducer.FindAsync(ratingsProducer.Id);
        
        result.Should().BeNull();
    }
}