using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.MediaRepository;

public class Media_DeleteTests : BaseRepositoryTest
{
    private readonly IMediaRepository _mediaRepository;

    public Media_DeleteTests()
    {
        _mediaRepository = new Infrastructure.Persistence.Repositories.MediaRepository(Context);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldDeleteMedia()
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
        await _mediaRepository.DeleteAsync(media);

        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.Media.FirstOrDefaultAsync(x => x.Id == media.Id);
        result.Should().BeNull();
    }
}