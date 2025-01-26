using Bogus;
using Domain.Entity;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.ProducerUserRepository;

public class ProducerUser_UpdateTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.ProducerUserRepository _repository;

    public ProducerUser_UpdateTests()
    {
        _repository = new Infrastructure.Persistence.Repositories.ProducerUserRepository(Context);
    }

    [Fact]
    public async Task ProducerUser_UpdateAsync_ShouldUpdateProducerUser()
    {
        // Arrange
        var faker = new Faker();
        var producer = new Producer
        {
            Id = faker.Random.Guid(),
            Name = faker.Company.CompanyName(),
            Description = faker.Company.CompanyName(),
        };
        var producerUser = new ProducerUser
        {
            Id = faker.Random.Guid(),
            Email = faker.Person.Email,
            Name = faker.Person.FirstName,
            Password = faker.Internet.Password(),
            Role = faker.PickRandom<Roles>(),
            ProducerId = producer.Id,
        };
        
        await Context.Producer.AddAsync(producer);
        await Context.ProducerUser.AddAsync(producerUser);
        
        await Context.SaveChangesAsync();
        
        producerUser.Name = faker.Company.CompanyName();
        producerUser.Email = faker.Person.Email;
        producerUser.Password = faker.Internet.Password();
        producerUser.Role = faker.PickRandom<Roles>();

        // Act

        await _repository.UpdateAsync(producerUser);
        
        // Assert
        
        var result = await Context.ProducerUser.FindAsync(producerUser.Id);
        
        result.Should().BeEquivalentTo(producerUser);
    }
}