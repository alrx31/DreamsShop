using Bogus;
using Domain.Entity;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.RatingsDreamsRepository;

public class RatingsDreams_UpdateTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.RatingsDreamsRepository _repository;

    public RatingsDreams_UpdateTests()
    {
        _repository = new Infrastructure.Persistence.Repositories.RatingsDreamsRepository(Context);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateRatingsDreams()
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

        // Act
        
        ratingsDreams.CreatedAt = faker.Date.Past();
        ratingsDreams.Value = faker.Random.Int();

        await _repository.UpdateAsync(ratingsDreams);
        
        // Assert
        
        var result = await Context.RatingsDreams.FindAsync(ratingsDreams.Id);
        result.Should().BeEquivalentTo(ratingsDreams);
    }
}