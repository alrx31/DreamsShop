using Bogus;
using Domain.Entity;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.IntegrationTests.Repositories;

public class OrderDreamRepositoryTests : BaseRepositoryTest
{
    private readonly OrderDreamRepository _repository;
    private readonly DreamRepository _dreamRepository;
    private readonly OrderRepository _orderRepository;

    public OrderDreamRepositoryTests()
    {
        _repository = new OrderDreamRepository(Context);
        _dreamRepository = new DreamRepository(Context);
        _orderRepository = new OrderRepository(Context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddOrderDream()
    {
        // Arrange
        var faker = new Faker();
        var dream = new Dream { DreamId = faker.Random.Guid(), Title = faker.Lorem.Sentence(), Description = faker.Lorem.Paragraph(), ImageFileName = "test.jpg" };
        var order = new Order { OrderId = faker.Random.Guid(), UserId = faker.Random.Guid() };
        await _dreamRepository.AddAsync(dream);
        await _orderRepository.AddAsync(order);
        await Context.SaveChangesAsync();
        
        var orderDream = new OrderDream
        {
            OrderId = order.OrderId,
            DreamId = dream.DreamId
        };

        // Act
        await _repository.AddAsync(orderDream);
        await Context.SaveChangesAsync();

        // Assert
        var result = await Context.OrderDreams.FindAsync(order.OrderId, dream.DreamId);
        result.Should().Be(orderDream);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldDeleteOrderDream()
    {
        // Arrange
        var faker = new Faker();
        var dream = new Dream { DreamId = faker.Random.Guid(), Title = faker.Lorem.Sentence(), Description = faker.Lorem.Paragraph(), ImageFileName = "test.jpg" };
        var order = new Order { OrderId = faker.Random.Guid(), UserId = faker.Random.Guid() };
        await _dreamRepository.AddAsync(dream);
        await _orderRepository.AddAsync(order);
        await Context.SaveChangesAsync();
        
        var orderDream = new OrderDream { OrderId = order.OrderId, DreamId = dream.DreamId };
        await _repository.AddAsync(orderDream);
        await Context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(orderDream);
        await Context.SaveChangesAsync();

        // Assert
        var result = await Context.OrderDreams.FindAsync(order.OrderId, dream.DreamId);
        result.Should().BeNull();
    }
}
