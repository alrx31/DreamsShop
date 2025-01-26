using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.DreamInOrderRepositoryTests;

public class DreamInOrder_DeleteTests : BaseRepositoryTest
{
    private readonly IDreamInOrderRepository _dreamInOrderRepository;
    
    public DreamInOrder_DeleteTests()
    {
        _dreamInOrderRepository = new DreamInOrderRepository(Context);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntityFromDatabase()
    {
        // Arrange
        var dreamInOrder = new Faker<DreamInOrder>().Generate();
        
        await Context.DreamInOrder.AddAsync(dreamInOrder);
        await Context.SaveChangesAsync();
        
        // Act
        await _dreamInOrderRepository.DeleteAsync(dreamInOrder);
        await Context.SaveChangesAsync();
        
        // Assert
        var entity = await Context.DreamInOrder.FindAsync(dreamInOrder.Id);
        
        entity.Should().BeNull();
    }
    
}