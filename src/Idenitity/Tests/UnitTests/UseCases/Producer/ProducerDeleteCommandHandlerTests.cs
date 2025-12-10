using Application.Exceptions;
using Application.UseCases.Producer.ProducerDelete;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IServices;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.Producer;

public class ProducerDeleteCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IProducerRepository> _producerRepository = new();
    private readonly Mock<IProducerUserRepository> _producerUserRepository = new();
    private readonly Mock<IHttpContextService> _httpContextService = new();
    private readonly ProducerDeleteCommandHandler _handler;

    public ProducerDeleteCommandHandlerTests()
    {
        _unitOfWork.Setup(u => u.ProducerRepository).Returns(_producerRepository.Object);
        _unitOfWork.Setup(u => u.ProducerUserRepository).Returns(_producerUserRepository.Object);

        _handler = new ProducerDeleteCommandHandler(_unitOfWork.Object, _httpContextService.Object);
    }

    [Fact]
    public async Task Handle_ShouldDelete_WhenAdminOwnsProducer()
    {
        var producerId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var producer = new Domain.Entity.Producer { Id = producerId, Title = "Studio" };
        var admin = new ProducerUser
        {
            Id = userId,
            ProducerId = producerId,
            Role = Roles.ProducerAdmin,
            Email = "admin@test.com",
            Name = "Admin",
            Password = "hashed"
        };

        _producerRepository.Setup(r => r.GetAsync(producerId, It.IsAny<CancellationToken>())).ReturnsAsync(producer);
        _httpContextService.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _producerUserRepository.Setup(r => r.GetAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(admin);

        await _handler.Handle(new ProducerDeleteCommand(producerId), CancellationToken.None);

        _producerRepository.Verify(r => r.DeleteAsync(producer, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenProducerMissing()
    {
        _producerRepository.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entity.Producer?)null);

        var act = async () => await _handler.Handle(new ProducerDeleteCommand(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUserUnauthorized()
    {
        var producerId = Guid.NewGuid();
        var producer = new Domain.Entity.Producer { Id = producerId, Title = "Studio" };

        _producerRepository.Setup(r => r.GetAsync(producerId, It.IsAny<CancellationToken>())).ReturnsAsync(producer);
        _httpContextService.Setup(s => s.GetCurrentUserId()).Returns((Guid?)null);

        var act = async () => await _handler.Handle(new ProducerDeleteCommand(producerId), CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUserNotOwnerOrAdmin()
    {
        var producerId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var producer = new Domain.Entity.Producer { Id = producerId, Title = "Studio" };
        var user = new ProducerUser
        {
            Id = userId,
            ProducerId = Guid.NewGuid(),
            Role = Roles.Producer,
            Email = "user@test.com",
            Name = "User",
            Password = "hashed"
        };

        _producerRepository.Setup(r => r.GetAsync(producerId, It.IsAny<CancellationToken>())).ReturnsAsync(producer);
        _httpContextService.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _producerUserRepository.Setup(r => r.GetAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var act = async () => await _handler.Handle(new ProducerDeleteCommand(producerId), CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenException>();
    }
}
