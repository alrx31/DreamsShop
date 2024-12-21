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
        var faker = new Faker();
        
        var orderTransactions = new List<OrderTransaction>
        {
            new OrderTransaction
            {
                Id = faker.Random.Guid(),
                Order_Id = faker.Random.Guid(),
                Status = faker.PickRandom<OrderTransactionStatuses>(),
                UpdatedAt = faker.Date.Past()
            },
            new OrderTransaction
            {
                Id = faker.Random.Guid(),
                Order_Id = faker.Random.Guid(),
                Status = faker.PickRandom<OrderTransactionStatuses>(),
                UpdatedAt = faker.Date.Past()
            },
            new OrderTransaction
            {
                Id = faker.Random.Guid(),
                Order_Id = faker.Random.Guid(),
                Status = faker.PickRandom<OrderTransactionStatuses>(),
                UpdatedAt = faker.Date.Past()
            }
        };
        
        await Context.OrderTransaction.AddRangeAsync(orderTransactions);
        await Context.SaveChangesAsync();
        
        // Act
        
        var result = await _orderTransactionRepository.GetAllAsync();
        
        // Assert
        
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(orderTransactions);
    }
}