using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.ConsumerUserRepositoryTests;

public class ConsumerUser_AddTests : BaseRepositoryTest
{
    private readonly ConsumerUserRepository _repository;

    public ConsumerUser_AddTests()
    {
        _repository = new ConsumerUserRepository(Context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddUser()
    {
        // Arrange
        var faker = new Faker();
        var user = new ConsumerUser
        {
            Id = faker.Random.Guid(),
            Email = faker.Person.Email,
            Password = faker.Internet.Password(),
            Name = faker.Person.FullName,
            Role = Roles.Consumer
        };
        
        // Act
        await _repository.AddAsync(user);
        await Context.SaveChangesAsync();

        // Assert
        var result = await Context.ConsumerUser.FindAsync(user.Id);
        
        result.Should().BeEquivalentTo(user);
    }
}