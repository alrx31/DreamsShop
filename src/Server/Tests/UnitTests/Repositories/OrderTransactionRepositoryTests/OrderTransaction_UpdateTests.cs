using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.OrderTransactionRepositoryTests;

public class OrderTransaction_UpdateTests : BaseRepositoryTest 
{
    private readonly OrderTransactionRepository _orderTransactionRepository;
    
    public OrderTransaction_UpdateTests()
    {
        _orderTransactionRepository = new OrderTransactionRepository(Context);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldUpdateOrderTransaction()
    {
        // Arrange
        var faker = new Faker();
        var orderTransaction = new Faker<OrderTransaction>().Generate();
        
        await Context.OrderTransaction.AddAsync(orderTransaction);
        await Context.SaveChangesAsync();
        
        var newStatus = faker.PickRandom<OrderTransactionStatuses>();
        
        // Act
        orderTransaction.Status = newStatus;
        await _orderTransactionRepository.UpdateAsync(orderTransaction);
        
        // Assert
        var result = await Context.OrderTransaction.FindAsync(orderTransaction.Id);
        
        result.Should().NotBeNull();
        result?.Status.Should().Be(newStatus);
    }
}