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
        var ratingDreams = new Faker<RatingsDreams>().Generate(3);
        
        await Context.RatingsDreams.AddRangeAsync(ratingDreams);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _ratingsDreams.GetAllAsync(1, 2);
        
        // Assert
        result.Should().BeEquivalentTo(ratingDreams);
    }
}