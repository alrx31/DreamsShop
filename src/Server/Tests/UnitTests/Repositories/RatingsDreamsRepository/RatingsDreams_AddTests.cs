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
        var faker = new Faker();
        
        var ratingDream = new RatingsDreams
        {
            Id = faker.Random.Guid(),
            DreamId = faker.Random.Guid(),
            ConsumerId = faker.Random.Guid(),
            Value = faker.Random.Int(),
            CreatedAt = faker.Date.Past()
        };
        
        // Act
        await _repository.AddAsync(ratingDream);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.RatingsDreams.FindAsync(ratingDream.Id);
        
        result.Should().BeEquivalentTo(ratingDream);
    }
}