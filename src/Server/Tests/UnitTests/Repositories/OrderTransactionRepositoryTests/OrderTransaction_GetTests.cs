using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.OrderTransactionRepositoryTests;

public class OrderTransaction_GetTests : BaseRepositoryTest
{
    private readonly OrderTransactionRepository _orderTransactionRepository;

    public OrderTransaction_GetTests()
    {
        _orderTransactionRepository = new OrderTransactionRepository(Context);
    }
    
    [Fact]
    public async Task GetAsync_ShouldReturnOrderTransaction()
    {
        // Arrange
        var faker = new Faker();
        
        var orderTransaction = new OrderTransaction
        {
            Id = faker.Random.Guid(),
            Order_Id = faker.Random.Guid(),
            Status = faker.PickRandom<OrderTransactionStatuses>(),
            UpdatedAt = faker.Date.Past()
        };
        
        await Context.OrderTransaction.AddAsync(orderTransaction);
        await Context.SaveChangesAsync();
        
        // Act
        
        var result = await _orderTransactionRepository.GetAsync(orderTransaction.Id);
        
        // Assert
        
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(orderTransaction);
    }
    
}