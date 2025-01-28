using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.DreamInOrderRepositoryTests;

public class DreamInOrder_GetTests : BaseRepositoryTest
{
    private readonly IDreamInOrderRepository _dreamInOrderRepository;
    
    public DreamInOrder_GetTests()
    {
        _dreamInOrderRepository = new DreamInOrderRepository(Context);
    }
    
    [Fact]
    public async Task GetAsync_ShouldReturnEntityFromDatabase()
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
        
        await Context.DreamInOrder.AddAsync(dreamInOrder);
        await Context.SaveChangesAsync();
        
        // Act
        var entity = await _dreamInOrderRepository.GetAsync(dreamInOrder.Id);
        
        // Assert
        entity.Should().NotBeNull();
        entity.Id.Should().Be(dreamInOrder.Id);
        entity.Dream_Id.Should().Be(dreamInOrder.Dream_Id);
        entity.Order_Id.Should().Be(dreamInOrder.Order_Id);
    }
    
    [Fact]
    public async Task GetRangeAsync_ShouldReturnRangeOfEntitiesFromDatabase()
    {
        // Arrange
        var faker = new Faker();
        
        var dreamInOrders = new List<DreamInOrder>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Dream_Id = Guid.NewGuid(),
                Order_Id = Guid.NewGuid(),
                AddDate = faker.Date.Past()
            },
            new()
            {
                Id = Guid.NewGuid(),
                Dream_Id = Guid.NewGuid(),
                Order_Id = Guid.NewGuid(),
                AddDate = faker.Date.Past()
            },
            new()
            {
                Id = Guid.NewGuid(),
                Dream_Id = Guid.NewGuid(),
                Order_Id = Guid.NewGuid(),
                AddDate = faker.Date.Past()
            }
        };
        
        await Context.DreamInOrder.AddRangeAsync(dreamInOrders);
        await Context.SaveChangesAsync();
        
        // Act
        var entities = await _dreamInOrderRepository.GetRangeAsync(0, 10);
        
        // Assert
        entities.Should().NotBeNull();
        entities.Should().HaveCount(3);
    }
}