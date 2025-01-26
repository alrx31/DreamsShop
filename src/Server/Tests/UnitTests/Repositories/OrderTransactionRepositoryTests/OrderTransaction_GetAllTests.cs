using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.OrderTransactionRepositoryTests;

public class OrderTransaction_GetAllTests : BaseRepositoryTest
{
    private readonly OrderTransactionRepository _orderTransactionRepository;
    
    public OrderTransaction_GetAllTests()
    {
        _orderTransactionRepository = new OrderTransactionRepository(Context);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllOrderTransactions()
    {
        // Arrange
        var orderTransactions = new Faker<OrderTransaction>().Generate(3);
        
        await Context.OrderTransaction.AddRangeAsync(orderTransactions);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _orderTransactionRepository.GetAllAsync();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(orderTransactions);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoOrderTransactions()
    {
        // Arrange
        // Act
        var result = await _orderTransactionRepository.GetAllAsync();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllOrderTransactions_WhenPageAndPageSizeProvided()
    {
        // Arrange
        var orderTransactions = new Faker<OrderTransaction>().Generate(3);
        
        await Context.OrderTransaction.AddRangeAsync(orderTransactions);
        await Context.SaveChangesAsync();
        
        // Act
        var result = await _orderTransactionRepository.GetAllAsync(0, 2);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(orderTransactions.Take(2));
    }
}