using Bogus;
using Domain.Entity;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.ProducerUserRepository;

public class ProducerUser_AddTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.ProducerUserRepository _repository;

    public ProducerUser_AddTests()
    {
        _repository = new Infrastructure.Persistence.Repositories.ProducerUserRepository(Context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddProducerUser()
    {
        // Arrange
        var producer = new Faker<Producer>().Generate();
        var producerUser = new Faker<ProducerUser>().RuleFor(p => p.ProducerId, producer.Id).Generate();
        
        await Context.AddAsync(producer);
        
        // Act
        await _repository.AddAsync(producerUser);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.ProducerUser.FindAsync(producerUser.Id);
        
        result.Should().BeEquivalentTo(producerUser);
    }
}