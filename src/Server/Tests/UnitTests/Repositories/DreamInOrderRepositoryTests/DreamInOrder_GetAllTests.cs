using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.DreamInOrderRepositoryTests;

public class DreamInOrder_GetAllTests : BaseRepositoryTest
{
    private readonly IDreamInOrderRepository _dreamInOrderRepository;
    
    public DreamInOrder_GetAllTests()
    {
        _dreamInOrderRepository = new DreamInOrderRepository(Context);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnEntitiesFromDatabase()
    {
        // Arrange
        var dreamInOrders = new Faker<DreamInOrder>().Generate(3);
        
        await Context.DreamInOrder.AddRangeAsync(dreamInOrders);
        await Context.SaveChangesAsync();
        
        // Act
        var entities = await _dreamInOrderRepository.GetAllAsync(1, 10);
        
        // Assert
        entities.Should().NotBeNull();
        entities.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task GetCountAsync_ShouldReturnCountOfEntitiesFromDatabase()
    {
        // Arrange
        var dreamInOrders = new Faker<DreamInOrder>().Generate(3);
        
        await Context.DreamInOrder.AddRangeAsync(dreamInOrders);
        await Context.SaveChangesAsync();
        
        // Act
        var count = await _dreamInOrderRepository.GetCountAsync();
        
        // Assert
        count.Should().Be(3);
    }
}