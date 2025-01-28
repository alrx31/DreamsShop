using Bogus;
using Domain.Entity;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.ProducerUserRepository;

public class ProducerUser_DeleteTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.ProducerUserRepository _repository;

    public ProducerUser_DeleteTests()
    {
        _repository = new Infrastructure.Persistence.Repositories.ProducerUserRepository(Context);
    }

    [Fact]
    public async Task DeleteAsync_NullProducerUser_ThrowsArgumentNullException()
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
        await _repository.DeleteAsync(producerUser);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.ProducerUser.FindAsync(producerUser.Id);
        
        result.Should().BeNull();
    }
}