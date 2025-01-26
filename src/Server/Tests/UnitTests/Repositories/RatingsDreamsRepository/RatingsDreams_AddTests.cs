using Bogus;
using Domain.Entity;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.RatingsDreamsRepository;

public class RatingsDreams_AddTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.RatingsDreamsRepository _repository;

    public RatingsDreams_AddTests()
    {
        _repository = new Infrastructure.Persistence.Repositories.RatingsDreamsRepository(Context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddRatingDream()
    {
        // Arrange
        var ratingDreams = new Faker<RatingsDreams>().Generate();
        
        // Act
        await _repository.AddAsync(ratingDreams);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.RatingsDreams.FindAsync(ratingDreams.Id);
        
        result.Should().BeEquivalentTo(ratingDreams);
    }
}