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
        var order = new Order
        {
            Id = faker.Random.Guid(),
            Cost = faker.Random.Decimal(),
            Consumer_Id = faker.Random.Guid(),
            Transaction_Id = faker.Random.Guid(),
        };
        
        await Context.Order.AddAsync(order);
        await Context.SaveChangesAsync();
        
        var updatedOrder = new Order
        {
            Id = order.Id,
            Cost = faker.Random.Decimal(),
            Consumer_Id = order.Consumer_Id,
            Transaction_Id = order.Transaction_Id,
        };
        
        order.Cost = updatedOrder.Cost;
        
        
        // Act
        await _orderRepository.UpdateAsync(order);
        await Context.SaveChangesAsync();
        
        // Assert
        var orderFromDb = await Context.Order.FindAsync(order.Id);
        
        orderFromDb.Should().NotBeNull();
        orderFromDb.Should().BeEquivalentTo(updatedOrder);
    }
}