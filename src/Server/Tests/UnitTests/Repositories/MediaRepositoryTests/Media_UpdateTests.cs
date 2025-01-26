using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.MediaRepositoryTests;

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
        var media = new Faker<Media>().Generate();
        
        await Context.Media.AddAsync(media);
        await Context.SaveChangesAsync();
        
        media.FileName = faker.Commerce.ProductName();
        media.FilePath = faker.Locale;
        media.FileExtension = faker.PickRandom(new[] { ".mp4", ".avi" });
        media.FileSize = faker.Random.Number(1, 10);
        media.File = faker.Random.Bytes(media.FileSize);
        
        // Act
        await _mediaRepository.UpdateAsync(media);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.Media.FirstOrDefaultAsync(x => x.Id == media.Id);
        
        result.Should().BeEquivalentTo(media);
    }
}