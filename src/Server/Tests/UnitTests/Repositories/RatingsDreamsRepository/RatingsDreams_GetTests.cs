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
        var ratingDreams = new Faker<RatingsDreams>().Generate();

        await Context.RatingsDreams.AddAsync(ratingDreams);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _repository.GetAsync(ratingDreams.Id);
        
        result.Should().BeEquivalentTo(ratingDreams);
    }
}