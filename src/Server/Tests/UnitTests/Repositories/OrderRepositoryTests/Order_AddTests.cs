using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.OrderRepositoryTests;

public class Order_AddTests : BaseRepositoryTest
{
    private readonly IOrderRepository _orderRepository;
    
    public Order_AddTests()
    {
        _orderRepository = new OrderRepository(Context);
    }
    
    [Fact]
    public async Task AddAsync_ShouldAddOrderToDatabase()
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
        
        // Act
        await _orderRepository.AddAsync(order);

        await Context.SaveChangesAsync();
        // Assert
        var orderFromDb = await Context.Order.FindAsync(order.Id);
        
        orderFromDb.Should().NotBeNull();
        orderFromDb.Should().BeEquivalentTo(order);
    }
}