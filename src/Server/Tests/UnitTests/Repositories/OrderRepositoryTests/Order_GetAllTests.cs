using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.OrderRepositoryTests;

public class Order_GetAllTests : BaseRepositoryTest
{
    private readonly IOrderRepository _orderRepository;
    
    public Order_GetAllTests()
    {
        _orderRepository = new OrderRepository(Context);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfOrders()
    {
        // Arrange
        var faker = new Faker();
        
        var orders = new List<Order>
        {
            new Order
            {
                Id = faker.Random.Guid(),
                Cost = faker.Random.Decimal(),
                Consumer_Id = faker.Random.Guid(),
                Transaction_Id = faker.Random.Guid(),
            },
            new Order
            {
                Id = faker.Random.Guid(),
                Cost = faker.Random.Decimal(),
                Consumer_Id = faker.Random.Guid(),
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
        var result = await _orderRepository.GetAllAsync(1, 10);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Should().BeEquivalentTo(orders);
    }
    
    [Fact]
    public async Task GetCountAsync_ShouldReturnCountOfOrders()
    {
        // Arrange
        var faker = new Faker();
     
        var orders = new List<Order>
        {
            new Order
            {
                Id = faker.Random.Guid(),
                Cost = faker.Random.Decimal(),
                Consumer_Id = faker.Random.Guid(),
                Transaction_Id = faker.Random.Guid(),
            },
            new Order
            {
                Id = faker.Random.Guid(),
                Cost = faker.Random.Decimal(),
                Consumer_Id = faker.Random.Guid(),
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
        var result = await _orderRepository.GetCountAsync();
        
        // Assert
        result.Should().Be(3);
    }
    
    [Fact]
    public async Task GetRangeAsync_ShouldReturnListOfOrders()
    {
        // Arrange
        var faker = new Faker();
        
        var orders = new List<Order>
        {
            new Order
            {
                Id = faker.Random.Guid(),
                Cost = faker.Random.Decimal(),
                Consumer_Id = faker.Random.Guid(),
                Transaction_Id = faker.Random.Guid(),
            },
            new Order
            {
                Id = faker.Random.Guid(),
                Cost = faker.Random.Decimal(),
                Consumer_Id = faker.Random.Guid(),
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
        var result = await _orderRepository.GetRangeAsync(0, 10);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Should().BeEquivalentTo(orders);
    }
}