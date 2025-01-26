using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories.OrderTransactionRepositoryTests;

public class OrderTransaction_AddTests : BaseRepositoryTest
{
    private readonly OrderTransactionRepository _orderTransactionRepository ;

    public OrderTransaction_AddTests()
    {
        _orderTransactionRepository = new OrderTransactionRepository(Context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddOrderTransaction()
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
        
        // Act
        
        await _orderTransactionRepository.AddAsync(orderTransaction);
        
        await Context.SaveChangesAsync();
        
        // Assert
        
        var result = await Context.OrderTransaction.FirstOrDefaultAsync(x => x.Id == orderTransaction.Id);
        
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(orderTransaction);
    }
}