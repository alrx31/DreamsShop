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
        var ratingDreams = new Faker<RatingsDreams>().Generate();
        
        await Context.RatingsDreams.AddAsync(ratingDreams);

        // Act
        ratingDreams.CreatedAt = faker.Date.Past();
        ratingDreams.Value = faker.Random.Int();

        await _repository.UpdateAsync(ratingDreams);
        
        // Assert
        var result = await Context.RatingsDreams.FindAsync(ratingDreams.Id);
        
        result.Should().BeEquivalentTo(ratingDreams);
    }
}