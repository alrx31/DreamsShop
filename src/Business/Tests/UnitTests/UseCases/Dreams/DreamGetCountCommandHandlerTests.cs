using Application.UseCases.Dreams.DreamGetCount;
using Bogus;
using Domain.IRepositories;
using FluentAssertions;
using Moq;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.UnitTests.UseCases.Dreams;

public class DreamGetCountCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IDreamRepository> _dreamRepositoryMock;
    private readonly DreamGetCountCommandHandler _handler;

    public DreamGetCountCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _dreamRepositoryMock = new Mock<IDreamRepository>();
        
        _unitOfWorkMock.Setup(u => u.DreamRepository).Returns(_dreamRepositoryMock.Object);
        
        _handler = new DreamGetCountCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnDreamCount()
    {
        // Arrange
        var faker = new Faker();
        var count = faker.Random.Int(1, 100);
        var command = new DreamGetCountCommand();

        _dreamRepositoryMock.Setup(r => r.GetCountAsync(CancellationToken.None)).ReturnsAsync(count);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(count);
        _dreamRepositoryMock.Verify(r => r.GetCountAsync(CancellationToken.None), Times.Once);
    }
}
