using System.Linq;
using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.IntegrationTests.Repositories;

public class DreamRepositoryTests : BaseRepositoryTest
{
    private readonly DreamRepository _repository;

    public DreamRepositoryTests()
    {
        _repository = new DreamRepository(Context);
    }

    [Fact]
    public async Task GetCountAsync_ShouldReturnCorrectCount()
    {
        // Arrange
        var faker = new Faker();
        var dreams = new List<Dream>
        {
            new() { DreamId = faker.Random.Guid(), Title = faker.Lorem.Sentence(), Description = faker.Lorem.Paragraph(), ImageFileName = "test1.jpg" },
            new() { DreamId = faker.Random.Guid(), Title = faker.Lorem.Sentence(), Description = faker.Lorem.Paragraph(), ImageFileName = "test2.jpg" }
        };
        await Context.Dream.AddRangeAsync(dreams);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetCountAsync();

        // Assert
        result.Should().Be(2);
    }

    [Fact]
    public async Task GetAsync_WithSkipAndTake_ShouldReturnCorrectRange()
    {
        // Arrange
        var faker = new Faker();
        var dreams = new List<Dream>
        {
            new() { DreamId = faker.Random.Guid(), Title = "A", Description = faker.Lorem.Paragraph(), ImageFileName = "test1.jpg" },
            new() { DreamId = faker.Random.Guid(), Title = "B", Description = faker.Lorem.Paragraph(), ImageFileName = "test2.jpg" },
            new() { DreamId = faker.Random.Guid(), Title = "C", Description = faker.Lorem.Paragraph(), ImageFileName = "test3.jpg" }
        };
        await Context.Dream.AddRangeAsync(dreams);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync<Dream>(skip: 1, take: 2);

        // Assert
        result.Should().HaveCount(2);
        result.First().Title.Should().Be("B");
    }
    
    [Fact]
    public async Task AddAsync_ShouldAddDream()
    {
        // Arrange
        var faker = new Faker();
        var dream = new Dream
        {
            DreamId = faker.Random.Guid(),
            Title = faker.Lorem.Sentence(),
            Description = faker.Lorem.Paragraph(),
            ImageFileName = "test.jpg"
        };

        // Act
        var dreamId = await _repository.AddAsync(dream);
        await Context.SaveChangesAsync();

        // Assert
        var result = await Context.Dream.FindAsync(dreamId);
        result.Should().Be(dream);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnDream()
    {
        // Arrange
        var faker = new Faker();
        var dream = new Dream
        {
            DreamId = faker.Random.Guid(),
            Title = faker.Lorem.Sentence(),
            Description = faker.Lorem.Paragraph(),
            ImageFileName = "test.jpg"
        };
        await Context.Dream.AddAsync(dream);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync(new[] { dream.DreamId });

        // Assert
        result.Should().Be(dream);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldUpdateDream()
    {
        // Arrange
        var faker = new Faker();
        var dream = new Dream
        {
            DreamId = faker.Random.Guid(),
            Title = faker.Lorem.Sentence(),
            Description = faker.Lorem.Paragraph(),
            ImageFileName = "test.jpg"
        };
        await Context.Dream.AddAsync(dream);
        await Context.SaveChangesAsync();
        
        var newTitle = faker.Lorem.Sentence();
        dream.Title = newTitle;

        // Act
        await _repository.UpdateAsync(dream);
        await Context.SaveChangesAsync();

        // Assert
        var result = await Context.Dream.FindAsync(dream.DreamId);

        result.Should().NotBeNull();
        result!.Title.Should().Be(newTitle);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldDeleteDream()
    {
        // Arrange
        var faker = new Faker();
        var dream = new Dream
        {
            DreamId = faker.Random.Guid(),
            Title = faker.Lorem.Sentence(),
            Description = faker.Lorem.Paragraph(),
            ImageFileName = "test.jpg"
        };
        await Context.Dream.AddAsync(dream);
        await Context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(dream);
        await Context.SaveChangesAsync();

        // Assert
        var result = await Context.Dream.FindAsync(dream.DreamId);
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task GetAsync_WithFilter_ShouldReturnCorrectRange()
    {
        // Arrange
        var faker = new Faker();
        var dreams = new List<Dream>
        {
            new() { DreamId = faker.Random.Guid(), Title = "A", Description = faker.Lorem.Paragraph(), ImageFileName = "test1.jpg" },
            new() { DreamId = faker.Random.Guid(), Title = "B", Description = faker.Lorem.Paragraph(), ImageFileName = "test2.jpg" },
            new() { DreamId = faker.Random.Guid(), Title = "C", Description = faker.Lorem.Paragraph(), ImageFileName = "test3.jpg" }
        };
        await Context.Dream.AddRangeAsync(dreams);
        await Context.SaveChangesAsync();
        var ids = dreams.Take(2).Select(d => d.DreamId).ToList();

        // Act
        var result = await _repository.GetAsync<Dream>(filter: d => ids.Contains(d.DreamId));

        // Assert
        result.Should().HaveCount(2);
        result.Select(d => d.DreamId).Should().BeEquivalentTo(ids);
    }
}
