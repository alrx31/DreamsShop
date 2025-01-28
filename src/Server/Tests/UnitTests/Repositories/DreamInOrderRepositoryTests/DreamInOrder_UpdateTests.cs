using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.DreamInOrderRepositoryTests;

public class DreamInOrder_UpdateTests : BaseRepositoryTest
{
    private readonly IDreamInOrderRepository _dreamInOrderRepository;
    
    public DreamInOrder_UpdateTests()
    {
        _dreamInOrderRepository = new DreamInOrderRepository(Context);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntityInDatabase()
    {
        // Arrange
        var faker = new Faker();
        
        var dreamInOrder = new DreamInOrder
        {
            Id = Guid.NewGuid(),
            Dream_Id = Guid.NewGuid(),
            Order_Id = Guid.NewGuid(),
            AddDate = faker.Date.Past()
        };
        
        await Context.DreamInOrder.AddAsync(dreamInOrder);
        await Context.SaveChangesAsync();
        
        var updatedDreamInOrder = new DreamInOrder
        {
            Id = dreamInOrder.Id,
            Dream_Id = Guid.NewGuid(),
            Order_Id = Guid.NewGuid(),
            AddDate = faker.Date.Past()
        };
        
        dreamInOrder.Order_Id = updatedDreamInOrder.Order_Id;
        dreamInOrder.Dream_Id = updatedDreamInOrder.Dream_Id;
        dreamInOrder.AddDate = updatedDreamInOrder.AddDate;
        
        // Act
        await _dreamInOrderRepository.UpdateAsync(dreamInOrder);
        await Context.SaveChangesAsync();
        
        // Assert
        var entity = await Context.DreamInOrder.FindAsync(dreamInOrder.Id);
        entity.Should().NotBeNull();
        entity.Id.Should().Be(updatedDreamInOrder.Id);
        entity.Dream_Id.Should().Be(updatedDreamInOrder.Dream_Id);
        entity.Order_Id.Should().Be(updatedDreamInOrder.Order_Id);
    }
}