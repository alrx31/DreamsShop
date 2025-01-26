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
            ImageMediaId = faker.Random.Guid(),
            PreviewMediaId = faker.Random.Guid(),
            ProducerId = faker.Random.Guid(),
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
        dream.ImageMediaId = newImageMediaId;
        dream.PreviewMediaId = newPreviewMediaId;
        dream.ProducerId = newProducerId;
        
        // Act
        
        await _dreamRepository.UpdateAsync(dream);
        
        await Context.SaveChangesAsync();
        
        // Assert
        
        var result = await Context.Dream.FirstOrDefaultAsync(x => x.Id == dream.Id);
        
        result.Should().NotBeNull();
        result.Title.Should().Be(newTitle);
        result.Desctiption.Should().Be(newDescription);
        result.ImageMediaId.Should().Be(newImageMediaId);
        result.PreviewMediaId.Should().Be(newPreviewMediaId);
        result.ProducerId.Should().Be(newProducerId);
    }
}