using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.ProducerUserRepositoryTests;

public class ProducerUserRepository_CrudTests : BaseRepositoryTest
{
    private readonly ProducerUserRepository _repository;
    private readonly ProducerRepository _producerRepository;
    private readonly Faker _faker = new();

    public ProducerUserRepository_CrudTests()
    {
        _repository = new ProducerUserRepository(Context);
        _producerRepository = new ProducerRepository(Context);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistUser()
    {
        var producer = await CreateProducerAsync();
        var user = BuildUser(producer.Id);

        await _repository.AddAsync(user);
        await Context.SaveChangesAsync();

        var stored = await Context.ProducerUser.FindAsync(user.Id);
        stored.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser()
    {
        var producer = await CreateProducerAsync();
        var user = BuildUser(producer.Id);

        await Context.ProducerUser.AddAsync(user);
        await Context.SaveChangesAsync();

        var stored = await _repository.GetByEmailAsync(user.Email);

        stored.Should().NotBeNull();
        stored!.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateFields()
    {
        var producer = await CreateProducerAsync();
        var user = BuildUser(producer.Id);

        await Context.ProducerUser.AddAsync(user);
        await Context.SaveChangesAsync();

        user.Name = _faker.Name.FullName();
        await _repository.UpdateAsync(user);
        await Context.SaveChangesAsync();

        var stored = await Context.ProducerUser.FindAsync(user.Id);
        stored!.Name.Should().Be(user.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveUser()
    {
        var producer = await CreateProducerAsync();
        var user = BuildUser(producer.Id);

        await Context.ProducerUser.AddAsync(user);
        await Context.SaveChangesAsync();

        await _repository.DeleteAsync(user);
        await Context.SaveChangesAsync();

        var stored = await Context.ProducerUser.FindAsync(user.Id);
        stored.Should().BeNull();
    }

    private async Task<Producer> CreateProducerAsync()
    {
        var producer = new Producer
        {
            Id = _faker.Random.Guid(),
            Title = _faker.Company.CompanyName(),
            Description = _faker.Lorem.Sentence()
        };

        await _producerRepository.AddAsync(producer);
        await Context.SaveChangesAsync();

        return producer;
    }

    private ProducerUser BuildUser(Guid producerId) =>
        new()
        {
            Id = _faker.Random.Guid(),
            ProducerId = producerId,
            Email = _faker.Internet.Email(),
            Password = _faker.Internet.Password(),
            Name = _faker.Name.FullName(),
            Role = Roles.Producer
        };
}
