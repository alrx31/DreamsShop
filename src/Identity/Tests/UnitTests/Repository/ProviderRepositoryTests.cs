using Bogus;
using DAL.Entities;
using DAL.IRepositories;
using DAL.Persistence;
using DAL.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repository;

public class ProviderRepositoryTests
{
    private readonly ApplicationDbContext _context;
    
    private readonly IProviderRepository _repository;
    
    public ProviderRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);

        _repository = new ProviderRepository(_context);
    }
    
    [Fact]
    public async Task AddProducer_Success_ShouldAddProducer()
    {
        // Arrange
        var faker = new Faker();

        var producer = new Provider
        {
            Name = faker.Company.CompanyName(),
            Description = faker.Company.CompanyName(),
            Staff = [],
            Raiting = faker.Random.Float(1,5)
        };
        
        // Act
        await _repository.AddProducer(producer);
        
        await _context.SaveChangesAsync();
        
        //Assert
        var result = await _context.Producers.FirstOrDefaultAsync(p => p.Name == producer.Name);
        
        result.Should().NotBeNull();
        result.Name.Should().Be(producer.Name);
        result.Staff.Should().BeEquivalentTo(producer.Staff);
        result.Raiting.Should().Be(producer.Raiting);
    }
    
    [Fact]
    public async Task GetProducerById_Success_ShouldReturnProducer()
    {
        // Arrange
        var faker = new Faker();
        
        var producer = new Provider
        {
            Name = faker.Company.CompanyName(),
            Description = faker.Company.CompanyName(),
            Staff = [],
            Raiting = faker.Random.Float(1,5)
        };
        
        await _context.Producers.AddAsync(producer);
        
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _repository.GetProvider(producer.Id);
        
        //Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(producer.Name);
        result.Staff.Should().BeEquivalentTo(producer.Staff);
        result.Raiting.Should().Be(producer.Raiting);
    }
    
    [Fact]
    public async Task GetProducerByName_Success_ShouldReturnProducer()
    {
        // Arrange
        var faker = new Faker();
        
        var producer = new Provider
        {
            Name = faker.Company.CompanyName(),
            Description = faker.Company.CompanyName(),
            Staff = [],
            Raiting = faker.Random.Float(1,5)
        };
        
        await _context.Producers.AddAsync(producer);
        
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _repository.GetProvider(producer.Name);
        
        //Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(producer.Name);
        result.Staff.Should().BeEquivalentTo(producer.Staff);
        result.Raiting.Should().Be(producer.Raiting);
    }
    
    [Fact]
    public async Task DeleteProducer_Success_ShouldDeleteProducer()
    {
        // Arrange
        var faker = new Faker();
        
        var producer = new Provider
        {
            Name = faker.Company.CompanyName(),
            Description = faker.Company.CompanyName(),
            Staff = [],
            Raiting = faker.Random.Float(1,5)
        };
        
        await _context.Producers.AddAsync(producer);
        
        await _context.SaveChangesAsync();
        
        // Act
        await _repository.DeleteProducer(producer);
        
        await _context.SaveChangesAsync();
        
        //Assert
        var result = await _context.Producers.FirstOrDefaultAsync(p => p.Name == producer.Name);
        
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task UpdateProducer_Success_ShouldUpdateProducer()
    {
        // Arrange
        var faker = new Faker();

        var producer = new Provider
        {
            Name = faker.Company.CompanyName(),
            Description = faker.Company.CompanyName(),
            Staff = [],
            Raiting = faker.Random.Float(1,5)
        };
        
        await _context.Producers.AddAsync(producer);
        
        await _context.SaveChangesAsync();

        producer.Name = faker.Company.CompanyName();
        producer.Staff = [];
        producer.Raiting = faker.Random.Float(1,5);
        
        // Act
        await _repository.UpdateProducer(producer);
        
        await _context.SaveChangesAsync();
        
        //Assert
        var result = await _context.Producers.FirstOrDefaultAsync(p => p.Name == producer.Name);
        
        result.Should().NotBeNull();
        result.Name.Should().Be(producer.Name);
        result.Staff.Should().BeEquivalentTo(producer.Staff);
        result.Raiting.Should().Be(producer.Raiting);
    } 
    
    
}