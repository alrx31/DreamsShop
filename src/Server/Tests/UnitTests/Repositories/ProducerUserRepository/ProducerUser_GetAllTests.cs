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
        var producer = new Faker<Producer>().Generate();
        var producerUsers = new Faker<ProducerUser>().RuleFor(p => p.ProducerId, producer.Id).Generate(2);
        
        await Context.Producer.AddAsync(producer);
        await Context.ProducerUser.AddRangeAsync(producerUsers);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _repository.GetAllAsync(1,2);
        
        // Assert
        result.Should().BeEquivalentTo(producerUsers);
    }
}