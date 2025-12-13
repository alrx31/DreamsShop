using Application.UseCases.ConsumerUserAuth.ConsumerUserRefreshAccessToken;
using Application.Exceptions;
using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IServices;
using Domain.Model;
using FluentAssertions;
using Moq;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ConsumerUserEntity = Domain.Entity.ConsumerUser;

namespace Tests.UnitTests.UseCases.ConsumerUserAuth;

public class ConsumerUserRefreshAccessTokenCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IConsumerUserRepository> _consumerUserRepositoryMock;
    private readonly Mock<ICookieService> _cookieServiceMock;
    private readonly Mock<IHttpContextService> _httpContextServiceMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly ConsumerUserRefreshAccessTokenCommandHandler _handler;

    public ConsumerUserRefreshAccessTokenCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _consumerUserRepositoryMock = new Mock<IConsumerUserRepository>();
        _cookieServiceMock = new Mock<ICookieService>();
        _httpContextServiceMock = new Mock<IHttpContextService>();
        _jwtServiceMock = new Mock<IJwtService>();
        
        _unitOfWorkMock.Setup(u => u.ConsumerUserRepository).Returns(_consumerUserRepositoryMock.Object);
        
        _handler = new ConsumerUserRefreshAccessTokenCommandHandler(
            _unitOfWorkMock.Object,
            _cookieServiceMock.Object,
            _httpContextServiceMock.Object,
            _jwtServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnNewAccessToken_WhenTokenIsValid()
    {
        // Arrange
        var faker = new Faker();
        var userId = faker.Random.Guid();
        var command = new ConsumerUserRefreshAccessTokenCommand();
        var user = new ConsumerUserEntity
        {
            Id = userId,
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Name.FullName(),
            Role = Roles.Consumer
        };
        var refreshTokenModel = new RefreshTokenCookieModel
        {
            Token = faker.Random.AlphaNumeric(30),
            Expires = DateTime.UtcNow.AddDays(7)
        };
        var cookieValue = JsonSerializer.Serialize(refreshTokenModel);
        var newAccessToken = faker.Random.AlphaNumeric(40);

        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _consumerUserRepositoryMock.Setup(r => r.GetAsync(userId, CancellationToken.None)).ReturnsAsync(user);
        _cookieServiceMock.Setup(c => c.GetCookie(userId.ToString())).Returns(cookieValue);
        _jwtServiceMock.Setup(j => j.GenerateJwtToken(user)).Returns(newAccessToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(newAccessToken);
        _httpContextServiceMock.Verify(s => s.GetCurrentUserId(), Times.Once);
        _consumerUserRepositoryMock.Verify(r => r.GetAsync(userId, CancellationToken.None), Times.Once);
        _cookieServiceMock.Verify(c => c.GetCookie(userId.ToString()), Times.Once);
        _jwtServiceMock.Verify(j => j.GenerateJwtToken(user), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserIdIsNull()
    {
        // Arrange
        var command = new ConsumerUserRefreshAccessTokenCommand();
        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns((Guid?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("Invalid token.");
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserNotFound()
    {
        // Arrange
        var faker = new Faker();
        var userId = faker.Random.Guid();
        var command = new ConsumerUserRefreshAccessTokenCommand();

        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _consumerUserRepositoryMock.Setup(r => r.GetAsync(userId, CancellationToken.None))
            .ReturnsAsync((ConsumerUserEntity?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("User not found.");
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenCookieIsMissing()
    {
        // Arrange
        var faker = new Faker();
        var userId = faker.Random.Guid();
        var command = new ConsumerUserRefreshAccessTokenCommand();
        var user = new ConsumerUserEntity
        {
            Id = userId,
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Name.FullName(),
            Role = Roles.Consumer
        };

        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _consumerUserRepositoryMock.Setup(r => r.GetAsync(userId, CancellationToken.None)).ReturnsAsync(user);
        _cookieServiceMock.Setup(c => c.GetCookie(userId.ToString())).Returns(string.Empty);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("Invalid token.");
    }
    
    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenRefreshTokenIsExpired()
    {
        // Arrange
        var faker = new Faker();
        var userId = faker.Random.Guid();
        var command = new ConsumerUserRefreshAccessTokenCommand();
        var user = new ConsumerUserEntity
        {
            Id = userId,
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Name = faker.Name.FullName(),
            Role = Roles.Consumer
        };
        var refreshTokenModel = new RefreshTokenCookieModel
        {
            Token = faker.Random.AlphaNumeric(30),
            Expires = DateTime.UtcNow.AddDays(-1) // Expired token
        };
        var cookieValue = JsonSerializer.Serialize(refreshTokenModel);

        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _consumerUserRepositoryMock.Setup(r => r.GetAsync(userId, CancellationToken.None)).ReturnsAsync(user);
        _cookieServiceMock.Setup(c => c.GetCookie(userId.ToString())).Returns(cookieValue);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("Token has expired.");
    }
}
