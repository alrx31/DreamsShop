using Application.DTO.ProducerUser;
using Application.Exceptions;
using Application.UseCases.ProducerUserAuth.ProducerUserLogin;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IServices;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Tests.UnitTests.UseCases.ProducerUserAuth;

public class ProducerUserLoginCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IProducerUserRepository> _repository = new();
    private readonly Mock<IJwtService> _jwtService = new();
    private readonly Mock<ICookieService> _cookieService = new();
    private readonly IConfiguration _configuration;
    private readonly ProducerUserLoginCommandHandler _handler;

    public ProducerUserLoginCommandHandlerTests()
    {
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:RefreshTokenExpiresInDays"] = "7"
            })
            .Build();

        _unitOfWork.Setup(u => u.ProducerUserRepository).Returns(_repository.Object);
        _handler = new ProducerUserLoginCommandHandler(
            _unitOfWork.Object,
            _jwtService.Object,
            _cookieService.Object,
            _configuration);
    }

    [Fact]
    public async Task Handle_ShouldReturnTokens_WhenUserExists()
    {
        var dto = new ProducerUserLoginDto { Email = "producer@test.com", Password = "pass" };
        var command = new ProducerUserLoginCommand(dto);
        var user = new ProducerUser
        {
            Id = Guid.NewGuid(),
            ProducerId = Guid.NewGuid(),
            Email = dto.Email,
            Name = "Producer",
            Password = "hashed",
            Role = Roles.Producer
        };

        _repository.Setup(r => r.GetByEmailAsync(dto.Email, CancellationToken.None)).ReturnsAsync(user);
        _jwtService.Setup(j => j.GenerateJwtToken(user)).Returns("access");
        _jwtService.Setup(j => j.GenerateRefreshToken()).Returns("refresh");

        var result = await _handler.Handle(command, CancellationToken.None);

        result.AccessToken.Should().Be("access");
        result.UserData.Should().Be(user);
        _cookieService.Verify(c => c.SetCookie(user.Id.ToString(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUserMissing()
    {
        var dto = new ProducerUserLoginDto { Email = "producer@test.com", Password = "pass" };
        var command = new ProducerUserLoginCommand(dto);

        _repository.Setup(r => r.GetByEmailAsync(dto.Email, CancellationToken.None)).ReturnsAsync((ProducerUser?)null);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedException>();
    }
}
