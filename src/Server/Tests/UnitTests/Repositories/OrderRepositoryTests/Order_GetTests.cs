using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.OrderRepositoryTests;

public class Order_GetTests : BaseRepositoryTest
{
    private readonly IOrderRepository _orderRepository;
    
    public Order_GetTests()
    {
        _orderRepository = new OrderRepository(Context);
    }
    
    [Fact]
    public async Task GetOrdersByUserId_ShouldReturnListOfOrders()
    {
        // Arrange
        var userId = new Faker().Random.Guid();
        var orders = new Faker<Order>()
            .RuleFor(o => o.Id, f => f.Random.Guid())
            .RuleFor(o => o.Cost, f => f.Random.Decimal())
            .RuleFor(o => o.Consumer_Id, (f, o) => userId)
            .RuleFor(o => o.Transaction_Id, f => f.Random.Guid())
            .Generate(3);
        
        await Context.Order.AddRangeAsync(orders);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _orderRepository.GetOrdersByUserId(userId);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(orders.Where(x => x.Consumer_Id == userId));
    }
    
    [Fact]
    public async Task GetAsync_ShouldReturnOrder()
    {
        // Arrange
        var order = new Faker<Order>().Generate();
        
        await Context.Order.AddAsync(order);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _orderRepository.GetAsync(order.Id);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(order);
    }
}