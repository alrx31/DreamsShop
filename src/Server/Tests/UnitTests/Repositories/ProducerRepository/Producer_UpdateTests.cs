using Bogus;
using Domain.Entity;
using FluentAssertions;

namespace Tests.UnitTests.Repositories.ProducerRepository;

public class Producer_UpdateTests : BaseRepositoryTest
{
    private readonly Infrastructure.Persistence.Repositories.ProducerRepository _repository;

    public Producer_UpdateTests()
    {
        _repository = new Infrastructure.Persistence.Repositories.ProducerRepository(Context);
    }

    [Fact]
    public async Task UpdateProducer_ShouldUpdateProducer()
    {
        var faker = new Faker();
        var producer = new Producer()
        {
            Id = faker.Random.Guid(),
            Name = faker.Company.CompanyName(),
            Description = faker.Random.Words(),
        };
        
        await Context.Producer.AddAsync(producer);
        await Context.SaveChangesAsync();
        
        // Act
        
        producer.Name = faker.Company.CompanyName();
        producer.Description = faker.Random.Words();
        
        await _repository.UpdateAsync(producer);
        
        // Assert
        
        var result = await Context.Producer.FindAsync(producer.Id);
        result.Should().BeEquivalentTo(producer);
        
    }
}