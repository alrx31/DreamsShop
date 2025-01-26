using Bogus;
using Domain.Entity;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.RatingsProducerRepository;

public class RatingsProducer_GetTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.RatingsProducerRepository _repository;

    public RatingsProducer_GetTests()
    {
        _repository = new Infrastructure.Persistence.Repositories.RatingsProducerRepository(Context);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnRatingsDreams()
    {
        // Arrange
        var ratingsProducer = new Faker<RatingsProducer>().Generate();
        
        await Context.RatingsProducer.AddAsync(ratingsProducer);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _repository.GetAsync(ratingsProducer.Id);
        
        // Assert
        result.Should().BeEquivalentTo(ratingsProducer);
    }
}