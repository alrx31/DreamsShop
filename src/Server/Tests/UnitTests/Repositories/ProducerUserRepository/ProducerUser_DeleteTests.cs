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
        var producer = new Faker<Producer>().Generate();
        var producerUser = new Faker<ProducerUser>().RuleFor(p => p.ProducerId, producer.Id).Generate();
        
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