using Application.Exceptions;
using Application.UseCases.Producer.ProducerGet;
using Domain.IRepositories;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.Producer;

public class ProducerGetCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IProducerRepository> _producerRepository = new();
    private readonly ProducerGetCommandHandler _handler;

    public ProducerGetCommandHandlerTests()
    {
        _unitOfWork.Setup(u => u.ProducerRepository).Returns(_producerRepository.Object);
        _handler = new ProducerGetCommandHandler(_unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProducer_WhenExists()
    {
        var id = Guid.NewGuid();
        var producer = new Domain.Entity.Producer { Id = id, Title = "Studio" };
        _producerRepository.Setup(r => r.GetAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(producer);

        var result = await _handler.Handle(new ProducerGetCommand(id), CancellationToken.None);

        result.Should().Be(producer);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenNotFound()
    {
        var id = Guid.NewGuid();
        _producerRepository.Setup(r => r.GetAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Domain.Entity.Producer?)null);

        var act = async () => await _handler.Handle(new ProducerGetCommand(id), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
