using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.DreamRepositoryTests;

public class Dream_AddTests : BaseRepositoryTest
{
    private readonly DreamRepository _dreamRepository;

    public Dream_AddTests()
    {
        _dreamRepository = new DreamRepository(Context);
    }
    
    [Fact]
    public async Task AddAsync_ShouldAddDream()
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
        
        // Act
        
        await _dreamRepository.AddAsync(dream);
        
        await Context.SaveChangesAsync();
        
        // Assert
        
        var result = await Context.Dream.FirstOrDefaultAsync(x => x.Id == dream.Id);
        
        result.Should().NotBeNull();
        result.Title.Should().Be(dream.Title);
        result.Desctiption.Should().Be(dream.Desctiption);
        result.Image_Media_Id.Should().Be(dream.Image_Media_Id);
        result.Preview_Media_Id.Should().Be(dream.Preview_Media_Id);
        result.Producer_Id.Should().Be(dream.Producer_Id);
    }
}