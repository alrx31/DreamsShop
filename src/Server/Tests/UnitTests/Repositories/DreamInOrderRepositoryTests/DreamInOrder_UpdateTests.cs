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
        var dreamInOrder = new Faker<DreamInOrder>().Generate();
        
        await Context.DreamInOrder.AddAsync(dreamInOrder);
        await Context.SaveChangesAsync();
        
        dreamInOrder.Order_Id = faker.Random.Guid();
        dreamInOrder.Dream_Id = faker.Random.Guid();
        dreamInOrder.AddDate = faker.Date.Past();
        
        // Act
        await _dreamInOrderRepository.UpdateAsync(dreamInOrder);
        await Context.SaveChangesAsync();
        
        // Assert
        var result = await Context.DreamInOrder.FindAsync(dreamInOrder.Id);
        
        result.Should().BeEquivalentTo(dreamInOrder);
    }
}