using Bogus;
using Domain.Entity;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.RatingsProducerRepository;

public class RatingsProducer_GetAllTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.RatingsProducerRepository _repository;

    public RatingsProducer_GetAllTests()
    {
        _repository = new Infrastructure.Persistence.Repositories.RatingsProducerRepository(Context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRatingsProducers()
    {
        // Arrange
        var ratingsProducerGenerator = new Faker<RatingsProducer>();
        var ratingsProducers = ratingsProducerGenerator.Generate(2);
        
        await Context.AddRangeAsync(ratingsProducers);
        await Context.SaveChangesAsync();
        
        // Act

        var result = await _repository.GetAllAsync(1,2);

        // Assert
        
        result.Should().BeEquivalentTo(ratingsProducers);
    }
}