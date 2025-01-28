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
        var faker = new Faker();
        var userId = faker.Random.Guid();
        
        var orders = new List<Order>
        {
            new Order
            {
                Id = faker.Random.Guid(),
                Cost = faker.Random.Decimal(),
                Consumer_Id = userId,
                Transaction_Id = faker.Random.Guid(),
            },
            new Order
            {
                Id = faker.Random.Guid(),
                Cost = faker.Random.Decimal(),
                Consumer_Id = userId,
                Transaction_Id = faker.Random.Guid(),
            },
            new Order
            {
                Id = faker.Random.Guid(),
                Cost = faker.Random.Decimal(),
                Consumer_Id = faker.Random.Guid(),
                Transaction_Id = faker.Random.Guid(),
            },
        };
        
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
        
        // Act
        var result = await _orderRepository.GetAsync(order.Id);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(order);
    }
}