using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.DreamRepositoryTests;

public class Dream_DeleteTests : BaseRepositoryTest
{
    private readonly DreamRepository _dreamRepository;

    public Dream_DeleteTests()
    {
        _dreamRepository = new DreamRepository(Context);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldDeleteDream()
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
        
        // Act
        await _dreamRepository.DeleteAsync(dream);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.Dream.FirstOrDefaultAsync(x => x.Id == dream.Id);
        
        result.Should().BeNull();
    }
}