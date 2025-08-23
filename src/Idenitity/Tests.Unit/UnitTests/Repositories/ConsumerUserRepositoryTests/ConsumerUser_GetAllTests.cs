using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.ConsumerUserRepositoryTests;

public class ConsumerUser_GetAllTests : BaseRepositoryTest
{
    private readonly ConsumerUserRepository _repository;

    public ConsumerUser_GetAllTests()
    {
        _repository = new ConsumerUserRepository(Context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUser()
    {
        // Arrange
        var faker = new Faker();
        var users = new List<ConsumerUser>
        {
            new ConsumerUser
            {
                Id = faker.Random.Guid(),
                Email = faker.Person.Email,
                Password = faker.Internet.Password(),
                Name = faker.Person.FullName,
                Role = Roles.Consumer
            },
            new ConsumerUser
            {
                Id = faker.Random.Guid(),
                Email = faker.Person.Email,
                Password = faker.Internet.Password(),
                Name = faker.Person.FullName,
                Role = Roles.Consumer
            }
        };
        
        await Context.ConsumerUser.AddRangeAsync(users);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _repository.GetAllAsync(1,2);

        // Assert
        result.Should().BeEquivalentTo(users);
    }
}