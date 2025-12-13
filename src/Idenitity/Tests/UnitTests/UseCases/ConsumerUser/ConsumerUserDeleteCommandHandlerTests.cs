using Application.UseCases.ConsumerUser.ConsumerUserDelete;
using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using FluentAssertions;
using Moq;
using Application.Exceptions;

namespace Tests.UnitTests.UseCases.ConsumerUser;

public class ConsumerUserDeleteCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IConsumerUserRepository> _repositoryMock;
    private readonly ConsumerUserDeleteCommandHandler _handler;

    public ConsumerUserDeleteCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _repositoryMock = new Mock<IConsumerUserRepository>();
        _unitOfWorkMock.Setup(u => u.ConsumerUserRepository).Returns(_repositoryMock.Object);
        _handler = new ConsumerUserDeleteCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteUser_WhenUserExists()
    {
        // Arrange
        var faker = new Faker();
        var user = new Domain.Entity.ConsumerUser
        {
            Id = faker.Random.Guid(),
            Email = faker.Person.Email,
            Password = faker.Internet.Password(),
            Name = faker.Person.FullName,
            Role = Roles.Consumer
        };

        _repositoryMock.Setup(r => r.GetAsync(user.Id, CancellationToken.None)).ReturnsAsync(user);
        _repositoryMock.Setup(r => r.DeleteAsync(user, CancellationToken.None)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(CancellationToken.None)).Returns(Task.FromResult(1));

        var command = new ConsumerUserDeleteCommand(user.Id);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.GetAsync(user.Id, CancellationToken.None), Times.Once);
        _repositoryMock.Verify(r => r.DeleteAsync(user, CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new ConsumerUserDeleteCommand(Guid.NewGuid());

        _repositoryMock.Setup(r => r.GetAsync(command.ConsumerUserId, CancellationToken.None)).ReturnsAsync((Domain.Entity.ConsumerUser)null!);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _repositoryMock.Verify(r => r.GetAsync(command.ConsumerUserId, CancellationToken.None), Times.Once);
        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Domain.Entity.ConsumerUser>(), CancellationToken.None), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Never);
    }
}
