using Application.UseCases.ConsumerUserAuth.ConsumerUserLogin;
using Application.DTO.ConsumerUser;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IServices;
using Domain.Model;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests.UnitTests.UseCases.ConsumerUserAuth;

public class ConsumerUserLoginCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IConsumerUserRepository> _consumerUserRepositoryMock;
    private readonly Mock<IPasswordManager> _passwordManagerMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<ICookieService> _cookieServiceMock;
    private readonly ConsumerUserLoginCommandHandler _handler;

    public ConsumerUserLoginCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _consumerUserRepositoryMock = new Mock<IConsumerUserRepository>();
        _passwordManagerMock = new Mock<IPasswordManager>();
        _jwtServiceMock = new Mock<IJwtService>();
        _configurationMock = new Mock<IConfiguration>();
        _cookieServiceMock = new Mock<ICookieService>();
        
        _unitOfWorkMock.Setup(u => u.ConsumerUserRepository).Returns(_consumerUserRepositoryMock.Object);
        
        _handler = new ConsumerUserLoginCommandHandler(
            _unitOfWorkMock.Object,
            _passwordManagerMock.Object,
            _jwtServiceMock.Object,
            _configurationMock.Object,
            _cookieServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnAuthResponse_WhenLoginIsSuccessful()
    {
        // Arrange
        var loginDto = new ConsumerUserLoginDto { Email = "test@test.com", Password = "password" };
        var command = new ConsumerUserLoginCommand(loginDto);
        var user = new ConsumerUser
        {
            Id = Guid.NewGuid(),
            Email = loginDto.Email,
            Password = "hashed_password",
            Name = "Test User",
            Role = Roles.Consumer
        };
        var token = "jwt_token";
        var refreshToken = "refresh_token";

        var refreshTokenExpiresInDaysSectionMock = new Mock<IConfigurationSection>();
        refreshTokenExpiresInDaysSectionMock.Setup(x => x.Value).Returns("7");
        _configurationMock.Setup(x => x.GetSection("Jwt:RefreshTokenExpiresInDays")).Returns(refreshTokenExpiresInDaysSectionMock.Object);


        _consumerUserRepositoryMock.Setup(r => r.GetByEmailAsync(loginDto.Email, CancellationToken.None)).ReturnsAsync(user);
        _passwordManagerMock.Setup(p => p.Verify(user.Password, loginDto.Password)).Returns(true);
        _jwtServiceMock.Setup(j => j.GenerateJwtToken(user)).Returns(token);
        _cookieServiceMock.Setup(c => c.GetCookie(user.Id.ToString())).Returns(string.Empty);
        _jwtServiceMock.Setup(j => j.GenerateRefreshToken()).Returns(refreshToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.AccessToken.Should().Be(token);
        result.UserData.Should().Be(user);
        _cookieServiceMock.Verify(c => c.SetCookie(user.Id.ToString(), It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserNotFound()
    {
        // Arrange
        var loginDto = new ConsumerUserLoginDto { Email = "test@test.com", Password = "password" };
        var command = new ConsumerUserLoginCommand(loginDto);

        _consumerUserRepositoryMock.Setup(r => r.GetByEmailAsync(loginDto.Email, CancellationToken.None))
            .ReturnsAsync((ConsumerUser?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }
    
    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenPasswordIsIncorrect()
    {
        // Arrange
        var loginDto = new ConsumerUserLoginDto { Email = "test@test.com", Password = "password" };
        var command = new ConsumerUserLoginCommand(loginDto);
        var user = new ConsumerUser
        {
            Id = Guid.NewGuid(),
            Email = loginDto.Email,
            Password = "hashed_password",
            Name = "Test User",
            Role = Roles.Consumer
        };

        _consumerUserRepositoryMock.Setup(r => r.GetByEmailAsync(loginDto.Email, CancellationToken.None)).ReturnsAsync(user);
        _passwordManagerMock.Setup(p => p.Verify(user.Password, loginDto.Password)).Returns(false);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}
