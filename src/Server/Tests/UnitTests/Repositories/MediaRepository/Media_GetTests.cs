using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.MediaRepository;

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
            File_Name = faker.System.FileName(),
            File_Path = faker.System.DirectoryPath(),
            File_Extension = faker.System.FileExt(),
            File_Size = faker.Random.Int(),
            File = faker.Random.Bytes(10),
        };
        
        await Context.Media.AddAsync(media);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _mediaRepository.GetAsync(media.Id);
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(media.Id);
        result.File_Name.Should().Be(media.File_Name);
        result.File_Path.Should().Be(media.File_Path);
        result.File_Extension.Should().Be(media.File_Extension);
        result.File_Size.Should().Be(media.File_Size);
        result.File.Should().BeEquivalentTo(media.File);
    }
}