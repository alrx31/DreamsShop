using Bogus;
using Domain.Entity;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.RatingsProducerRepository;

public class RatingsProducer_UpdateTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.RatingsProducerRepository _repository;

    public RatingsProducer_UpdateTests()
    {
        _repository = new Infrastructure.Persistence.Repositories.RatingsProducerRepository(Context);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateRatingsProducer()
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
        
        await Context.RatingsProducer.AddAsync(ratingsProducer);
        await Context.SaveChangesAsync();
        
        // Act
        ratingsProducer.CreatedAt = faker.Date.Past();
        ratingsProducer.Value = faker.Random.Int();
        
        await _repository.UpdateAsync(ratingsProducer);
        
        // Assert
        var result = await Context.RatingsProducer.FindAsync(ratingsProducer.Id);
        
        result.Should().BeEquivalentTo(ratingsProducer);
    }
}