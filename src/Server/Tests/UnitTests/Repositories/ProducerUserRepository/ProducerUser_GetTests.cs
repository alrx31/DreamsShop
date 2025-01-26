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
        var producer = new Faker<Producer>().Generate();
        var producerUser = new Faker<ProducerUser>().RuleFor(p => p.ProducerId, producer.Id).Generate();

        await Context.Producer.AddAsync(producer);
        await Context.ProducerUser.AddAsync(producerUser);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _repository.GetAsync(producerUser.Id);
        
        // Assert
        result.Should().BeEquivalentTo(producerUser);
    }
}