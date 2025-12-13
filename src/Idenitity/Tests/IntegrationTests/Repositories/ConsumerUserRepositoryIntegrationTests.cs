using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;
using Tests.IntegrationTests.Fixtures;
using Xunit;

namespace Tests.IntegrationTests.Repositories;

[Collection("IntegrationTests")]
public class ConsumerUserRepositoryIntegrationTests(PostgreSqlFixture fixture)
{
    private readonly PostgreSqlFixture _fixture = fixture;

    [Fact]
    public async Task AddAndGetAsync_ShouldPersistUser()
    {
        await _fixture.ResetDatabaseAsync();
        await using var context = _fixture.CreateContext();
        var repository = new ConsumerUserRepository(context);
        var faker = new Faker();

        var user = new ConsumerUser
        {
            Id = faker.Random.Guid(),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Name.FullName(),
            Role = Roles.Consumer
        };

        await repository.AddAsync(user);
        await context.SaveChangesAsync();

        var stored = await repository.GetAsync(user.Id);

        stored.Should().NotBeNull();
        stored.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser_WhenExists()
    {
        await _fixture.ResetDatabaseAsync();
        await using var context = _fixture.CreateContext();
        var repository = new ConsumerUserRepository(context);
        var faker = new Faker();

        var user = new ConsumerUser
        {
            Id = faker.Random.Guid(),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Name.FullName(),
            Role = Roles.Consumer
        };

        await repository.AddAsync(user);
        await context.SaveChangesAsync();

        var stored = await repository.GetByEmailAsync(user.Email);

        stored.Should().NotBeNull();
        stored!.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task UpdateAsync_ShouldChangeStoredFields()
    {
        await _fixture.ResetDatabaseAsync();
        await using var context = _fixture.CreateContext();
        var repository = new ConsumerUserRepository(context);
        var faker = new Faker();

        var user = new ConsumerUser
        {
            Id = faker.Random.Guid(),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Name.FullName(),
            Role = Roles.Consumer
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
        var repository = new ConsumerUserRepository(context);
        var faker = new Faker();

        var user = new ConsumerUser
        {
            Id = faker.Random.Guid(),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Name.FullName(),
            Role = Roles.Consumer
        };

        await repository.AddAsync(user);
        await context.SaveChangesAsync();

        await repository.DeleteAsync(user);
        await context.SaveChangesAsync();

        var stored = await repository.GetAsync(user.Id);
        stored.Should().BeNull();
    }
}
