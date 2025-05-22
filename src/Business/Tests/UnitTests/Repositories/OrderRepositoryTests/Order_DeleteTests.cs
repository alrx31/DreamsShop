using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.OrderRepositoryTests;

public class Order_DeleteTests : BaseRepositoryTest
{
    private readonly IOrderRepository _orderRepository;
    
    public Order_DeleteTests()
    {
        _orderRepository = new OrderRepository(Context);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldDeleteOrderFromDatabase()
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
        await _orderRepository.DeleteAsync(order);
        await Context.SaveChangesAsync();
        
        // Assert
        var orderFromDb = await Context.Order.FindAsync(order.Id);
        
        orderFromDb.Should().BeNull();
    }
}