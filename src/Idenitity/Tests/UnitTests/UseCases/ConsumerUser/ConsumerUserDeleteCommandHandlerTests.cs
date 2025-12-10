using Application.Exceptions;
using Application.UseCases.ConsumerUser.ConsumerUserDelete;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.ConsumerUserCommands;

public class ConsumerUserDeleteCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IConsumerUserRepository> _repository = new();
    private readonly ConsumerUserDeleteCommandHandler _handler;

    public ConsumerUserDeleteCommandHandlerTests()
    {
        _unitOfWork.Setup(x => x.ConsumerUserRepository).Returns(_repository.Object);
        _handler = new ConsumerUserDeleteCommandHandler(_unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteUser_WhenFound()
    {
        var user = new ConsumerUser
        {
            Id = Guid.NewGuid(),
            Email = "user@test.com",
            Name = "User",
            Password = "hashed",
            Role = Roles.Consumer
        };
        var command = new ConsumerUserDeleteCommand(user.Id);

        _repository.Setup(r => r.GetAsync(user.Id, CancellationToken.None)).ReturnsAsync(user);

        await _handler.Handle(command, CancellationToken.None);

        _repository.Verify(r => r.DeleteAsync(user, CancellationToken.None), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUserNotFound()
    {
        var command = new ConsumerUserDeleteCommand(Guid.NewGuid());
        _repository.Setup(r => r.GetAsync(command.ConsumerUserId, CancellationToken.None)).ReturnsAsync((ConsumerUser?)null);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
