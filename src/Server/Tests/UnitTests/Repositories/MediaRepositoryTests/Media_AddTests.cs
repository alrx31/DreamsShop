using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.MediaRepositoryTests;

public class Media_AddTests : BaseRepositoryTest
{
    private readonly IMediaRepository _mediaRepository;
    
    public Media_AddTests()
    {
        _mediaRepository = new Infrastructure.Persistence.Repositories.MediaRepository(Context);
    }
    
    [Fact]
    public async Task AddAsync_ShouldAddMedia()
    {
        // Arrange
        var media = new Faker<Media>().Generate();
        
        // Act
        await _mediaRepository.AddAsync(media);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.Media.FindAsync(media.Id);
        
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(media);
    }
}