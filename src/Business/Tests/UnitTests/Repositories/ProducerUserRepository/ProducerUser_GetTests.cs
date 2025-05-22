using Bogus;
using Domain.Entity;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.ProducerUserRepository;

public class ProducerUser_GetTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.ProducerUserRepository _repository;

    public ProducerUser_GetTests()
    {
        _repository = new Infrastructure.Persistence.Repositories.ProducerUserRepository(Context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducerUsers()
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
        
        // Act
        var result = await _repository.GetAsync(producerUser.Id);
        
        // Assert
        result.Should().BeEquivalentTo(producerUser);
    }
}