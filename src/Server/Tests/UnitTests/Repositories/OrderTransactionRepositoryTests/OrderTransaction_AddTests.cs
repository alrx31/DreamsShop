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
        var orderTransaction = new Faker<OrderTransaction>().Generate();
        
        // Act
        await _orderTransactionRepository.AddAsync(orderTransaction);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.OrderTransaction.FindAsync(orderTransaction.Id);
        
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(orderTransaction);
    }
}