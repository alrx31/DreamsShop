using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.DreamRepositoryTests;

public class Dream_UpdateTests : BaseRepositoryTest
{
    private readonly DreamRepository _dreamRepository;

    public Dream_UpdateTests()
    {
        _dreamRepository = new DreamRepository(Context);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldUpdateDream()
    {
        // Arrange
        var faker = new Faker();
        
        var dream = new Dream
        {
            Id = faker.Random.Guid(),
            Title = faker.Lorem.Sentence(),
            Desctiption = faker.Lorem.Paragraph(),
            Image_Media_Id = faker.Random.Guid(),
            Preview_Media_Id = faker.Random.Guid(),
            Producer_Id = faker.Random.Guid(),
        };
        
        await Context.Dream.AddAsync(dream);
        
        await Context.SaveChangesAsync();
        
        var newTitle = faker.Lorem.Sentence();
        var newDescription = faker.Lorem.Paragraph();
        var newImageMediaId = faker.Random.Guid();
        var newPreviewMediaId = faker.Random.Guid();
        var newProducerId = faker.Random.Guid();
        
        dream.Title = newTitle;
        dream.Desctiption = newDescription;
        dream.Image_Media_Id = newImageMediaId;
        dream.Preview_Media_Id = newPreviewMediaId;
        dream.Producer_Id = newProducerId;
        
        // Act
        
        await _dreamRepository.UpdateAsync(dream);
        
        await Context.SaveChangesAsync();
        
        // Assert
        
        var result = await Context.Dream.FirstOrDefaultAsync(x => x.Id == dream.Id);
        
        result.Should().NotBeNull();
        result.Title.Should().Be(newTitle);
        result.Desctiption.Should().Be(newDescription);
        result.Image_Media_Id.Should().Be(newImageMediaId);
        result.Preview_Media_Id.Should().Be(newPreviewMediaId);
        result.Producer_Id.Should().Be(newProducerId);
    }
}