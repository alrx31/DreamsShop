using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.MediaRepository;

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
        
        // Act
        await _mediaRepository.AddAsync(media);
        
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.Media.FirstOrDefaultAsync(x => x.Id == media.Id);
        result.Should().NotBeNull();
        result.Id.Should().Be(media.Id);
        result.FileName.Should().Be(media.FileName);
        result.FilePath.Should().Be(media.FilePath);
        result.FileExtension.Should().Be(media.FileExtension);
        result.FileSize.Should().Be(media.FileSize);
        result.File.Should().BeEquivalentTo(media.File);
    }
}