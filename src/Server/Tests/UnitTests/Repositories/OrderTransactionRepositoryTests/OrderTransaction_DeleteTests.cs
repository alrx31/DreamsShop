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
        var faker = new Faker();
        
        var orderTransaction = new OrderTransaction
        {
            Id = faker.Random.Guid(),
            OrderId = faker.Random.Guid(),
            Status = faker.PickRandom<OrderTransactionStatuses>(),
            UpdatedAt = faker.Date.Past()
        };
        
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