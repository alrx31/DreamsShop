using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;
using Tests.IntegrationTests.Fixtures;
using Xunit;

namespace Tests.IntegrationTests.Repositories;

[Collection("IntegrationTests")]
public class ProducerUserRepositoryIntegrationTests(PostgreSqlFixture fixture)
{
    private readonly PostgreSqlFixture _fixture = fixture;

    [Fact]
    public async Task AddAndGetAsync_ShouldPersistUser()
    {
        await _fixture.ResetDatabaseAsync();
        await using var context = _fixture.CreateContext();
        var repository = new ProducerUserRepository(context);
        var producerRepository = new ProducerRepository(context);
        var faker = new Faker();

        var producer = new Producer
        {
            Id = faker.Random.Guid(),
            Title = faker.Company.CompanyName(),
            Description = faker.Lorem.Sentence()
        };

        await producerRepository.AddAsync(producer);
        await context.SaveChangesAsync();

        var user = new ProducerUser
        {
            Id = faker.Random.Guid(),
            ProducerId = producer.Id,
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Name.FullName(),
            Role = Roles.Producer
        };

        await repository.AddAsync(user);
        await context.SaveChangesAsync();

        var stored = await repository.GetAsync(user.Id);

        stored.Should().NotBeNull();
        stored.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser()
    {
        await _fixture.ResetDatabaseAsync();
        await using var context = _fixture.CreateContext();
        var repository = new ProducerUserRepository(context);
        var producerRepository = new ProducerRepository(context);
        var faker = new Faker();

        var producer = new Producer
        {
            Id = faker.Random.Guid(),
            Title = faker.Company.CompanyName(),
            Description = faker.Lorem.Sentence()
        };

        await producerRepository.AddAsync(producer);
        await context.SaveChangesAsync();

        var user = new ProducerUser
        {
            Id = faker.Random.Guid(),
            ProducerId = producer.Id,
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Name.FullName(),
            Role = Roles.Producer
        };

        await repository.AddAsync(user);
        await context.SaveChangesAsync();

        var stored = await repository.GetByEmailAsync(user.Email);

        stored.Should().NotBeNull();
        stored!.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistChanges()
    {
        await _fixture.ResetDatabaseAsync();
        await using var context = _fixture.CreateContext();
        var repository = new ProducerUserRepository(context);
        var producerRepository = new ProducerRepository(context);
        var faker = new Faker();

        var producer = new Producer
        {
            Id = faker.Random.Guid(),
            Title = faker.Company.CompanyName(),
            Description = faker.Lorem.Sentence()
        };

        await producerRepository.AddAsync(producer);
        await context.SaveChangesAsync();

        var user = new ProducerUser
        {
            Id = faker.Random.Guid(),
            ProducerId = producer.Id,
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Name.FullName(),
            Role = Roles.Producer
        };

        await repository.AddAsync(user);
        await context.SaveChangesAsync();

        user.Name = faker.Name.FullName();
        await repository.UpdateAsync(user);
        await context.SaveChangesAsync();

        var stored = await repository.GetAsync(user.Id);

        stored.Should().NotBeNull();
        stored!.Name.Should().Be(user.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        await _fixture.ResetDatabaseAsync();
        await using var context = _fixture.CreateContext();
        var repository = new ProducerUserRepository(context);
        var producerRepository = new ProducerRepository(context);
        var faker = new Faker();

        var producer = new Producer
        {
            Id = faker.Random.Guid(),
            Title = faker.Company.CompanyName(),
            Description = faker.Lorem.Sentence()
        };

        await producerRepository.AddAsync(producer);
        await context.SaveChangesAsync();

        var user = new ProducerUser
        {
            Id = faker.Random.Guid(),
            ProducerId = producer.Id,
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Name.FullName(),
            Role = Roles.Producer
        };

        await repository.AddAsync(user);
        await context.SaveChangesAsync();

        await repository.DeleteAsync(user);
        await context.SaveChangesAsync();

        var stored = await repository.GetAsync(user.Id);
        stored.Should().BeNull();
    }
}
