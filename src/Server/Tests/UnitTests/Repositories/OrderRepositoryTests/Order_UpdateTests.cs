using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.OrderRepositoryTests;

public class Order_UpdateTests : BaseRepositoryTest
{
    private readonly IOrderRepository _orderRepository;
    
    public Order_UpdateTests()
    {
        _orderRepository = new OrderRepository(Context);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldUpdateOrderInDatabase()
    {
        // Arrange
        var faker = new Faker();
        var order = new Faker<Order>().Generate();
        
        await Context.Order.AddAsync(order);
        await Context.SaveChangesAsync();
        
        order.Cost = faker.Random.Decimal();
        
        // Act
        await _orderRepository.UpdateAsync(order);
        await Context.SaveChangesAsync();
        
        // Assert
        var orderFromDb = await Context.Order.FindAsync(order.Id);
        
        orderFromDb.Should().NotBeNull();
        orderFromDb.Should().BeEquivalentTo(order);
    }
}