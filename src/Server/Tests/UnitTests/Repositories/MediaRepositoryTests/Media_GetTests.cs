using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.MediaRepositoryTests;

public class Media_GetTests : BaseRepositoryTest
{
    private readonly IMediaRepository _mediaRepository;
    
    public Media_GetTests()
    {
        _mediaRepository = new Infrastructure.Persistence.Repositories.MediaRepository(Context);
    }
    
    [Fact]
    public async Task GetAsync_ShouldReturnMedia()
    {
        // Arrange
        var media = new Faker<Media>().Generate();
        
        await Context.Media.AddAsync(media);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _mediaRepository.GetAsync(media.Id);
        
        // Assert
        result.Should().BeEquivalentTo(media);
    }
}