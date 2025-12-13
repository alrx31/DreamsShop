using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;
using Tests.IntegrationTests.Fixtures;
using Xunit;

namespace Tests.IntegrationTests.Repositories;

[Collection("IntegrationTests")]
public class ProducerRepositoryIntegrationTests(PostgreSqlFixture fixture)
{
    private readonly PostgreSqlFixture _fixture = fixture;

    [Fact]
    public async Task AddAndGetAsync_ShouldPersistProducer()
    {
        await _fixture.ResetDatabaseAsync();
        await using var context = _fixture.CreateContext();
        var repository = new ProducerRepository(context);
        var faker = new Faker();

        var producer = new Producer
        {
            Id = faker.Random.Guid(),
            Title = faker.Company.CompanyName(),
            Description = faker.Lorem.Sentence(),
            Rating = faker.Random.Decimal(1, 5)
        };

        await repository.AddAsync(producer);
        await context.SaveChangesAsync();

        var stored = await repository.GetAsync(producer.Id);

        stored.Should().NotBeNull();
        stored.Should().BeEquivalentTo(producer);
    }

    [Fact]
    public async Task GetByTitleAsync_ShouldReturnProducer()
    {
        await _fixture.ResetDatabaseAsync();
        await using var context = _fixture.CreateContext();
        var repository = new ProducerRepository(context);
        var faker = new Faker();

        var producer = new Producer
        {
            Id = faker.Random.Guid(),
            Title = faker.Company.CompanyName(),
            Description = faker.Lorem.Sentence(),
            Rating = faker.Random.Decimal(1, 5)
        };

        await repository.AddAsync(producer);
        await context.SaveChangesAsync();

        var stored = await repository.GetByTitleAsync(producer.Title);

        stored.Should().NotBeNull();
        stored!.Title.Should().Be(producer.Title);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistChanges()
    {
        await _fixture.ResetDatabaseAsync();
        await using var context = _fixture.CreateContext();
        var repository = new ProducerRepository(context);
        var faker = new Faker();

        var producer = new Producer
        {
            Id = faker.Random.Guid(),
            Title = faker.Company.CompanyName(),
            Description = faker.Lorem.Sentence(),
            Rating = faker.Random.Decimal(1, 5)
        };

        await repository.AddAsync(producer);
        await context.SaveChangesAsync();

        producer.Description = faker.Lorem.Paragraph();
        await repository.UpdateAsync(producer);
        await context.SaveChangesAsync();

        var stored = await repository.GetAsync(producer.Id);

        stored.Should().NotBeNull();
        stored!.Description.Should().Be(producer.Description);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        await _fixture.ResetDatabaseAsync();
        await using var context = _fixture.CreateContext();
        var repository = new ProducerRepository(context);
        var faker = new Faker();

        var producer = new Producer
        {
            Id = faker.Random.Guid(),
            Title = faker.Company.CompanyName(),
            Description = faker.Lorem.Sentence(),
            Rating = faker.Random.Decimal(1, 5)
        };

        await repository.AddAsync(producer);
        await context.SaveChangesAsync();

        await repository.DeleteAsync(producer);
        await context.SaveChangesAsync();

        var stored = await repository.GetAsync(producer.Id);
        stored.Should().BeNull();
    }
}
