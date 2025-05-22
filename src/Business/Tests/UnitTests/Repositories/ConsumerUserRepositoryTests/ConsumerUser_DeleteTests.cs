using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.ConsumerUserRepositoryTests;

public class ConsumerUser_DeleteTests : BaseRepositoryTest
{
    private readonly ConsumerUserRepository _repository;

    public ConsumerUser_DeleteTests()
    {
        _repository = new ConsumerUserRepository(Context);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteUser()
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
        await _repository.DeleteAsync(user);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.ConsumerUser.FindAsync(user.Id);
        
        result.Should().BeNull();
    }
}