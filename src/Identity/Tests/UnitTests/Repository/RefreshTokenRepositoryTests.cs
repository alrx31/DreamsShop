using Bogus;
using DAL.Entities;
using DAL.IRepositories;
using DAL.Persistence;
using DAL.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repository;

public class RefreshTokenRepositoryTests
{
    private readonly ApplicationDbContext _context;

    private readonly IRefreshTokenRepository _repository;

    public RefreshTokenRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);

        _repository = new RefreshTokenRepository(_context);
    }
    
    [Fact]
    public async Task GetRefreshTokenByToken_Success_WhenTokenExists()
    {
        // Arrange
        
        var faker = new Faker();

        var refreshToken = new RefreshTokenModel
        {
            Token = faker.Random.String(),
            UserId = faker.Random.Guid()
        };
        
        await _context.RefreshTokens.AddAsync(refreshToken);
        
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _repository.GetRefreshToken(refreshToken.Token);
        
        // Assert
        
        result.Should().NotBeNull();
        result.Token.Should().Be(refreshToken.Token);
        result.UserId.Should().Be(refreshToken.UserId);
    }

    [Fact]
    public async Task GetRefreshTokenById_Success_WhenTokenExist()
    {
        // Arrange
        
        var faker = new Faker();
        
        var refreshToken = new RefreshTokenModel
        {
            Token = faker.Random.String(),
            UserId = faker.Random.Guid()
        };
        
        await _context.RefreshTokens.AddAsync(refreshToken);
        
        await _context.SaveChangesAsync();
        
        var tokenmodel = _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken.Token);
        
        // Act
        
        var result = await _repository.GetRefreshToken(tokenmodel.Result.Id);
        
        // Assert
        
        result.Should().NotBeNull();
        result.Token.Should().Be(refreshToken.Token);
        result.UserId.Should().Be(refreshToken.UserId);
    }

    [Fact]
    public async Task AddRefreshToken_Success_ShouldAddToken()
    {
        var faker = new Faker();
        
        var refreshToken = new RefreshTokenModel
        {
            Token = faker.Random.String(),
            UserId = faker.Random.Guid()
        };
        
        // Act
        
        await _repository.AddRefreshToken(refreshToken);
        
        await _context.SaveChangesAsync();
        
        // Assert
        
        var result = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken.Token);
        
        result.Should().NotBeNull();
        result.Token.Should().Be(refreshToken.Token);
        result.UserId.Should().Be(refreshToken.UserId);
    }
    
    [Fact]
    public async Task DeleteRefreshToken_Success_ShouldDeleteToken()
    {
        // Arrange
        
        var faker = new Faker();
        
        var refreshToken = new RefreshTokenModel
        {
            Token = faker.Random.String(),
            UserId = faker.Random.Guid()
        };
        
        await _context.RefreshTokens.AddAsync(refreshToken);
        
        await _context.SaveChangesAsync();
        
        // Act
        
        await _repository.DeleteRefreshToken(refreshToken);
        
        await _context.SaveChangesAsync();
        
        // Assert
        
        var result = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken.Token);
        
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task UpdateRefreshToken_Success_ShouldUpdateToken()
    {
        // Arrange
        
        var faker = new Faker();
        
        var refreshToken = new RefreshTokenModel
        {
            Token = faker.Random.String(),
            UserId = faker.Random.Guid()
        };
        
        await _context.RefreshTokens.AddAsync(refreshToken);
        
        await _context.SaveChangesAsync();
        
        var tokenmodel = _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken.Token);
        
        refreshToken.Token = faker.Random.String();
        
        // Act
        
        await _repository.UpdateRefreshToken(refreshToken);
        
        await _context.SaveChangesAsync();
        
        // Assert
        
        var result = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken.Token);
        
        result.Should().NotBeNull();
        result.Token.Should().Be(refreshToken.Token);
        result.UserId.Should().Be(refreshToken.UserId);
    }
}