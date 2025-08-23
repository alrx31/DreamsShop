using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.ConsumerUserRepositoryTests;

public class ConsumerUser_GetTests : BaseRepositoryTest
{
    private readonly ConsumerUserRepository _repository;

    public ConsumerUser_GetTests()
    {
        _repository = new ConsumerUserRepository(Context);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnUser()
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
        
        await Context.ConsumerUser.AddAsync(user);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _repository.GetAsync(user.Id);

        // Assert
        result.Should().BeEquivalentTo(user);
    }
}