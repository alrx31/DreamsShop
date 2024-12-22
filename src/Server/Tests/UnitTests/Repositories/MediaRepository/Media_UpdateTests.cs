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
            File_Name = faker.System.FileName(),
            File_Path = faker.System.DirectoryPath(),
            File_Extension = faker.System.FileExt(),
            File_Size = faker.Random.Int(),
            File = faker.Random.Bytes(10),
        };
        
        await Context.Media.AddAsync(media);
        await Context.SaveChangesAsync();
        
        var updatedMedia = new Media
        {
            Id = media.Id,
            File_Name = faker.System.FileName(),
            File_Path = faker.System.DirectoryPath(),
            File_Extension = faker.System.FileExt(),
            File_Size = faker.Random.Int(),
            File = faker.Random.Bytes(10),
        };
        
        media.File_Name = updatedMedia.File_Name;
        media.File_Path = updatedMedia.File_Path;
        media.File_Extension = updatedMedia.File_Extension;
        media.File_Size = updatedMedia.File_Size;
        media.File = updatedMedia.File;
        
        // Act
        await _mediaRepository.UpdateAsync(media);
        
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.Media.FirstOrDefaultAsync(x => x.Id == media.Id);
        result.Should().NotBeNull();
        result.Id.Should().Be(updatedMedia.Id);
        result.File_Name.Should().Be(updatedMedia.File_Name);
        result.File_Path.Should().Be(updatedMedia.File_Path);
        result.File_Extension.Should().Be(updatedMedia.File_Extension);
        result.File_Size.Should().Be(updatedMedia.File_Size);
        result.File.Should().BeEquivalentTo(updatedMedia.File);
    }
    
}