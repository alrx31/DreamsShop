using Bogus;
using Domain.Entity;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.RatingsDreamsRepository;

public class RatingsDreams_DeleteTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.RatingsDreamsRepository _ratingsDreamsRepository;

    public RatingsDreams_DeleteTests()
    {
        _ratingsDreamsRepository  = new Infrastructure.Persistence.Repositories.RatingsDreamsRepository(Context);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteRatingsDreams()
    {
        // Arrange
        var ratingDreams = new Faker<RatingsDreams>().Generate();

        await Context.AddAsync(ratingDreams);
        await Context.SaveChangesAsync();
        
        // Act
        await _ratingsDreamsRepository.DeleteAsync(ratingDreams);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.RatingsDreams.FindAsync(ratingDreams.Id);
        
        result.Should().BeNull();
    }
}