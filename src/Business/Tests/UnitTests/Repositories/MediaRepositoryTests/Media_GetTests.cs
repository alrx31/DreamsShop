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
        var faker = new Faker();
        
        var media = new Media
        {
            Id = faker.Random.Guid(),
            FileName = faker.System.FileName(),
            FilePath = faker.System.DirectoryPath(),
            FileExtension = faker.System.FileExt(),
            FileSize = faker.Random.Int(),
            File = faker.Random.Bytes(10),
        };
        
        await Context.Media.AddAsync(media);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _mediaRepository.GetAsync(media.Id);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(media);
    }
}