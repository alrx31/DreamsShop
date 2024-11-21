using DAL.IRepositories;
using Bogus;
using DAL.Entities;
using DAL.Persistence;
using DAL.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repository;

public class UserRepositoryTests
{
    private readonly ApplicationDbContext _context;

    private readonly IUserRepository _repository;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);

        _repository = new UserRepository(_context);
    }

    [Fact]
    public async Task AddUser_Success_ShouldAddUser()
    {
        // Arrange
        var faker = new Faker();

        var user = new User
        {
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Name.FullName(),
            Role = Roles.CONSUMER
        };
        
        // Act
        await _repository.AddUser(user);
        
        await _context.SaveChangesAsync();
        
        //Assert
        var result = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        
        result.Should().NotBeNull();
        result.Email.Should().Be(user.Email);
        result.Name.Should().Be(user.Name);
        result.Role.Should().Be(user.Role);
    }
    
    [Fact]
    public async Task GetUserById_Success_ShouldReturnUser()
    {
        // Arrange
        var faker = new Faker();

        var user = new User
        {
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Name.FullName(),
            Role = Roles.CONSUMER
        };
        
        await _context.Users.AddAsync(user);
        
        await _context.SaveChangesAsync();

        user.Id = (await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email)).Id;
        
        // Act
        var result = await _repository.GetUser(user.Id);
        
        //Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(user.Email);
        result.Name.Should().Be(user.Name);
        result.Role.Should().Be(user.Role);
    }

    [Fact]
    public async Task GetUserByEmail()
    {
        // Arrange
        var faker = new Faker();

        var user = new User
        {
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Name.FullName(),
            Role = Roles.CONSUMER
        };
        
        await _context.Users.AddAsync(user);
        
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _repository.GetUser(user.Email);
        
        //Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(user.Email);
        result.Name.Should().Be(user.Name);
        result.Role.Should().Be(user.Role);
    }
    
    [Fact]
    public async Task DeleteUser_Success_ShouldDeleteUser()
    {
        // Arrange
        var faker = new Faker();

        var user = new User
        {
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Name.FullName(),
            Role = Roles.CONSUMER
        };
        
        await _context.Users.AddAsync(user);
        
        await _context.SaveChangesAsync();

        user.Id = (await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email)).Id;
        
        // Act
        await _repository.DeleteUser(user);
        
        await _context.SaveChangesAsync();
        
        //Assert
        var result = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task UpdateUser_Success_ShouldUpdateUser()
    {
        // Arrange
        var faker = new Faker();

        var user = new User
        {
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Name.FullName(),
            Role = Roles.CONSUMER
        };
        
        await _context.Users.AddAsync(user);
        
        await _context.SaveChangesAsync();

        user.Id = (await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email)).Id;
        
        user.Email = faker.Internet.Email();
        user.Name = faker.Name.FullName();
        user.Password = faker.Internet.Password();
        
        // Act
        await _repository.UpdateUser(user);
        
        await _context.SaveChangesAsync();
        
        //Assert
        var result = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        
        result.Should().NotBeNull();
        result.Email.Should().Be(user.Email);
        result.Name.Should().Be(user.Name);
        result.Role.Should().Be(user.Role);
    }
    
}