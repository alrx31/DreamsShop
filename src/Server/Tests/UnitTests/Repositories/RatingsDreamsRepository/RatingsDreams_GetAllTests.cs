using Bogus;
using Domain.Entity;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.RatingsDreamsRepository;

public class RatingsDreams_GetAllTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.RatingsDreamsRepository _ratingsDreams;

    public RatingsDreams_GetAllTests()
    {
        _ratingsDreams = new Infrastructure.Persistence.Repositories.RatingsDreamsRepository(Context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRatingsDreams()
    {
        // Arrange
        var faker = new Faker();
        var ratingsDreams = new List<RatingsDreams>
        {
            new RatingsDreams
            {
                Id = faker.Random.Guid(),
                DreamId = faker.Random.Guid(),
                ConsumerId = faker.Random.Guid(),
                Value = faker.Random.Int(),
                CreatedAt = faker.Date.Past(),
            },
            new RatingsDreams
            {
                Id = faker.Random.Guid(),
                DreamId = faker.Random.Guid(),
                ConsumerId = faker.Random.Guid(),
                Value = faker.Random.Int(),
                CreatedAt = faker.Date.Past(),
            }
        };
        
        await Context.RatingsDreams.AddRangeAsync(ratingsDreams);
        await Context.SaveChangesAsync();
        
        // Act

        var result = await _ratingsDreams.GetAllAsync(1, 2);
        
        // Assert
        
        result.Should().BeEquivalentTo(ratingsDreams);
    }
}