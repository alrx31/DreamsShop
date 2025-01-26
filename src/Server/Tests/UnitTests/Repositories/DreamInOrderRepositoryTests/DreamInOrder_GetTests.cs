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
        var dreamInOrder = new Faker<DreamInOrder>().Generate();
        
        await Context.DreamInOrder.AddAsync(dreamInOrder);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _dreamInOrderRepository.GetAsync(dreamInOrder.Id);
        
        // Assert
        result.Should().BeEquivalentTo(dreamInOrder);
    }
    
    [Fact]
    public async Task GetRangeAsync_ShouldReturnRangeOfEntitiesFromDatabase()
    {
        // Arrange
        var dreamInOrders = new Faker<DreamInOrder>().Generate(3);
        
        await Context.DreamInOrder.AddRangeAsync(dreamInOrders);
        await Context.SaveChangesAsync();
        
        // Act
        var entities = await _dreamInOrderRepository.GetRangeAsync(0, 10);
        
        // Assert
        entities.Should().NotBeNull();
        entities.Should().HaveCount(3);
    }
}