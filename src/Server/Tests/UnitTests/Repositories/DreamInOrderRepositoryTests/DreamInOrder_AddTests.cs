using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;

namespace Tests.UnitTests.Repositories.DreamInOrderRepositoryTests;

public class DreamInOrder_AddTests : BaseRepositoryTest
{
    private readonly IDreamInOrderRepository _dreamInOrderRepository;
    
    public DreamInOrder_AddTests()
    {
        _dreamInOrderRepository = new DreamInOrderRepository(Context);
    }
    
    [Fact]
    public async Task AddAsync_ShouldAddEntityToDatabase()
    {
        // Arrange
        var dreamInOrder = new Faker<DreamInOrder>().Generate();
        
        // Act
        await _dreamInOrderRepository.AddAsync(dreamInOrder);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.DreamInOrder.FindAsync(dreamInOrder.Id);
        
        result.Should().BeEquivalentTo(dreamInOrder);
    }
}