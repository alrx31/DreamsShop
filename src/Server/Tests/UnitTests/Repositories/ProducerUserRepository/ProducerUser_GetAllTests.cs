using Bogus;
using Domain.Entity;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.ProducerUserRepository;

public class ProducerUser_GetAllTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.ProducerUserRepository _repository;

    public ProducerUser_GetAllTests()
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
        
        var producerUsers = new List<ProducerUser>
        {
            new ProducerUser
            {
                Id = faker.Random.Guid(),
                Email = faker.Person.Email,
                Name = faker.Person.FirstName,
                Password = faker.Internet.Password(),
                Role = faker.PickRandom<Roles>(),
                ProducerId = producer.Id,
            },
            new ProducerUser
            {
                Id = faker.Random.Guid(),
                Email = faker.Person.Email,
                Name = faker.Person.FirstName,
                Password = faker.Internet.Password(),
                Role = faker.PickRandom<Roles>(),
                ProducerId = producer.Id,
            }
        };
        
        await Context.Producer.AddAsync(producer);
        await Context.ProducerUser.AddRangeAsync(producerUsers);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _repository.GetAllAsync(1,2);
        
        // Assert
        result.Should().BeEquivalentTo(producerUsers);
    }
}