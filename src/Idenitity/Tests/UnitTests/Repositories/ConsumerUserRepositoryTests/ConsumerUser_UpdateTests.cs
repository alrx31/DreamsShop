using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.ConsumerUserRepositoryTests;

public class ConsumerUser_UpdateTests : BaseRepositoryTest
{
    private readonly ConsumerUserRepository _repository;

    public ConsumerUser_UpdateTests()
    {
        _repository = new ConsumerUserRepository(Context);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser()
    {
        // Arrange
        var faker = new Faker();
        var user = new ConsumerUser
        {
            Id = faker.Random.Guid(),
            Email = faker.Person.Email,
            Password = faker.Internet.Password(),
            Name = faker.Person.FullName,
            Role = Roles.CONSUMER
        };
        
        await Context.ConsumerUser.AddAsync(user);
        await Context.SaveChangesAsync();
        
        // Act
        user.Email = faker.Person.Email;
        user.Name = faker.Person.FullName;
        user.Password = faker.Internet.Password();
        
        await _repository.UpdateAsync(user);

        // Assert
        var result = await Context.ConsumerUser.FindAsync(user.Id);
        
        result.Should().BeEquivalentTo(user);
    }
    
}