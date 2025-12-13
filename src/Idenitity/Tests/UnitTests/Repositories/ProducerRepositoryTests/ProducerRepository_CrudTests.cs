using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.ProducerRepositoryTests;

public class ProducerRepository_CrudTests : BaseRepositoryTest
{
    private readonly ProducerRepository _repository;
    private readonly Faker _faker = new();

    public ProducerRepository_CrudTests()
    {
        _repository = new ProducerRepository(Context);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistProducer()
    {
        var producer = new Producer
        {
            Id = _faker.Random.Guid(),
            Title = _faker.Company.CompanyName(),
            Description = _faker.Lorem.Sentence()
        };

        await _repository.AddAsync(producer);
        await Context.SaveChangesAsync();

        var stored = await Context.Producer.FindAsync(producer.Id);
        stored.Should().BeEquivalentTo(producer);
    }

    [Fact]
    public async Task GetByTitleAsync_ShouldReturnProducer()
    {
        var title = _faker.Company.CompanyName();
        var producer = new Producer
        {
            Id = _faker.Random.Guid(),
            Title = title,
            Description = _faker.Lorem.Sentence()
        };

        await Context.Producer.AddAsync(producer);
        await Context.SaveChangesAsync();

        var stored = await _repository.GetByTitleAsync(title);

        stored.Should().NotBeNull();
        stored!.Title.Should().Be(title);
    }

    [Fact]
    public async Task UpdateAsync_ShouldChangeEntity()
    {
        var producer = new Producer
        {
            Id = _faker.Random.Guid(),
            Title = _faker.Company.CompanyName(),
            Description = _faker.Lorem.Sentence()
        };

        await Context.Producer.AddAsync(producer);
        await Context.SaveChangesAsync();

        producer.Description = _faker.Lorem.Paragraph();
        await _repository.UpdateAsync(producer);
        await Context.SaveChangesAsync();

        var stored = await Context.Producer.FindAsync(producer.Id);
        stored!.Description.Should().Be(producer.Description);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveProducer()
    {
        var producer = new Producer
        {
            Id = _faker.Random.Guid(),
            Title = _faker.Company.CompanyName(),
            Description = _faker.Lorem.Sentence()
        };

        await Context.Producer.AddAsync(producer);
        await Context.SaveChangesAsync();

        await _repository.DeleteAsync(producer);
        await Context.SaveChangesAsync();

        var stored = await Context.Producer.FindAsync(producer.Id);
        stored.Should().BeNull();
    }
}
