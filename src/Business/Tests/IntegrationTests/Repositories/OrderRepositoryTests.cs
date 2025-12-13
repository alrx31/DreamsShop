using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.IntegrationTests.Repositories;

public class OrderRepositoryTests : BaseRepositoryTest
{
    private readonly OrderRepository _repository;
    private readonly DreamRepository _dreamRepository;

    public OrderRepositoryTests()
    {
        _repository = new OrderRepository(Context);
        _dreamRepository = new DreamRepository(Context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddOrder()
    {
        // Arrange
        var faker = new Faker();
        var order = new Order
        {
            OrderId = faker.Random.Guid(),
            UserId = faker.Random.Guid()
        };

        // Act
        var createdOrder = await _repository.AddAsync(order);
        await Context.SaveChangesAsync();

        // Assert
        var result = await Context.Orders.FindAsync(createdOrder.OrderId);
        result.Should().Be(order);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnOrder()
    {
        // Arrange
        var faker = new Faker();
        var order = new Order
        {
            OrderId = faker.Random.Guid(),
            UserId = faker.Random.Guid()
        };
        await Context.Orders.AddAsync(order);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync(new[] { order.OrderId });

        // Assert
        result.Should().Be(order);
    }
    
    [Fact]
    public async Task GetOrdersByUser_ShouldReturnOrders()
    {
        // Arrange
        var faker = new Faker();
        var userId = faker.Random.Guid();
        var orders = new List<Order>
        {
            new() { OrderId = faker.Random.Guid(), UserId = userId },
            new() { OrderId = faker.Random.Guid(), UserId = userId }
        };
        await Context.Orders.AddRangeAsync(orders);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetOrdersByUser(userId, 0, 10, default);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateOrder()
    {
        // Arrange
        var faker = new Faker();
        var order = new Order
        {
            OrderId = faker.Random.Guid(),
            UserId = faker.Random.Guid()
        };
        await Context.Orders.AddAsync(order);
        await Context.SaveChangesAsync();
        
        var newUserId = faker.Random.Guid();
        order.UserId = newUserId;

        // Act
        await _repository.UpdateAsync(order);
        await Context.SaveChangesAsync();

        // Assert
        var result = await Context.Orders.FindAsync(order.OrderId);

        result.Should().NotBeNull();
        result!.UserId.Should().Be(newUserId);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldDeleteOrder()
    {
        // Arrange
        var faker = new Faker();
        var order = new Order
        {
            OrderId = faker.Random.Guid(),
            UserId = faker.Random.Guid()
        };
        await Context.Orders.AddAsync(order);
        await Context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(order);
        await Context.SaveChangesAsync();

        // Assert
        var result = await Context.Orders.FindAsync(order.OrderId);
        result.Should().BeNull();
    }
}
