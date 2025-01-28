using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.MediaRepository;

public class Media_UpdateTests : BaseRepositoryTest
{
    private readonly IMediaRepository _mediaRepository;
    
    public Media_UpdateTests()
    {
        _mediaRepository = new Infrastructure.Persistence.Repositories.MediaRepository(Context);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldUpdateMedia()
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
        
        var updatedMedia = new Media
        {
            Id = media.Id,
            FileName = faker.System.FileName(),
            FilePath = faker.System.DirectoryPath(),
            FileExtension = faker.System.FileExt(),
            FileSize = faker.Random.Int(),
            File = faker.Random.Bytes(10),
        };
        
        media.FileName = updatedMedia.FileName;
        media.FilePath = updatedMedia.FilePath;
        media.FileExtension = updatedMedia.FileExtension;
        media.FileSize = updatedMedia.FileSize;
        media.File = updatedMedia.File;
        
        // Act
        await _mediaRepository.UpdateAsync(media);
        
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.Media.FirstOrDefaultAsync(x => x.Id == media.Id);
        result.Should().NotBeNull();
        result.Id.Should().Be(updatedMedia.Id);
        result.FileName.Should().Be(updatedMedia.FileName);
        result.FilePath.Should().Be(updatedMedia.FilePath);
        result.FileExtension.Should().Be(updatedMedia.FileExtension);
        result.FileSize.Should().Be(updatedMedia.FileSize);
        result.File.Should().BeEquivalentTo(updatedMedia.File);
    }
    
}