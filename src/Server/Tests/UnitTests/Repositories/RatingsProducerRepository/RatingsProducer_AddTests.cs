using Bogus;
using Domain.Entity;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.RatingsProducerRepository;

public class RatingsProducer_AddTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.RatingsProducerRepository _repository;

    public RatingsProducer_AddTests()
    {
        _repository = new Infrastructure.Persistence.Repositories.RatingsProducerRepository(Context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddRatingsProducer()
    {
        // Arrange
        var ratingsProducer = new Faker<RatingsProducer>().Generate();
        
        // Act
        await _repository.AddAsync(ratingsProducer);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.RatingsProducer.FindAsync(ratingsProducer.Id);
        
        result.Should().BeEquivalentTo(ratingsProducer);
    }
}