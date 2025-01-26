using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.OrderTransactionRepositoryTests;

public class OrderTransaction_DeleteTests : BaseRepositoryTest
{
    private readonly OrderTransactionRepository _orderTransactionRepository;
    
    public OrderTransaction_DeleteTests()
    {
        _orderTransactionRepository = new OrderTransactionRepository(Context);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldDeleteOrderTransaction()
    {
        // Arrange
        var orderTransaction = new Faker<OrderTransaction>().Generate();
        
        await Context.OrderTransaction.AddAsync(orderTransaction);
        await Context.SaveChangesAsync();
        
        // Act
        await _orderTransactionRepository.DeleteAsync(orderTransaction);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.OrderTransaction.FindAsync(orderTransaction.Id);
        
        result.Should().BeNull();
    }
}