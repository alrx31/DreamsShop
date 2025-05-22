using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.DreamInOrderRepositoryTests;

public class DreamInOrder_AddTests : BaseRepositoryTest
{
    private readonly IDreamInOrderRepository _dreamInOrderRepository;
    
    public DreamInOrder_AddTests()
    {
        _dreamInOrderRepository = new DreamInOrderRepository(Context);
    }
    
    [Fact]
    public async Task AddAsync_ShouldAddEntityToDatabase()
    {
        // Arrange
        var faker = new Faker();
        
        var dreamInOrder = new DreamInOrder
        {
            Id = Guid.NewGuid(),
            Dream_Id = Guid.NewGuid(),
            Order_Id = Guid.NewGuid(),
            AddDate = faker.Date.Past()
        };
        
        // Act
        await _dreamInOrderRepository.AddAsync(dreamInOrder);
        await Context.SaveChangesAsync();
        
        // Assert
        var entity = await Context.DreamInOrder.FindAsync(dreamInOrder.Id);
        entity.Should().NotBeNull();
        entity?.Id.Should().Be(dreamInOrder.Id);
        entity?.Dream_Id.Should().Be(dreamInOrder.Dream_Id);
        entity?.Order_Id.Should().Be(dreamInOrder.Order_Id);
    }
}