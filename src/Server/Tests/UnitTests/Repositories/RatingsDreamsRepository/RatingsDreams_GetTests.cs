using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence;

namespace Tests.UnitTests.Repositories.RatingsDreamsRepository;

public class RatingsDreams_GetTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.RatingsDreamsRepository _repository;

    public RatingsDreams_GetTests()
    {
        _repository = new Infrastructure.Persistence.Repositories.RatingsDreamsRepository(Context);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnRatingsDreams()
    {
        // Arrange
        var faker = new Faker();
        var ratingsDreams = new RatingsDreams
        {
            Id = faker.Random.Guid(),
            DreamId = faker.Random.Guid(),
            ConsumerId = faker.Random.Guid(),
            Value = faker.Random.Int(),
            CreatedAt = faker.Date.Past(),
        };
        
        await Context.RatingsDreams.AddAsync(ratingsDreams);
        await Context.SaveChangesAsync();
        
        // Act
        
        var result = await _repository.GetAsync(ratingsDreams.Id);
        
        result.Should().BeEquivalentTo(ratingsDreams);
    }
}