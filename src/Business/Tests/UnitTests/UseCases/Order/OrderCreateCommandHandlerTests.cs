using Application.UseCases.Order.CreateOrder;
using Application.DTO;
using Application.DTO.Order;
using Application.Exceptions;
using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IService;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.UnitTests.UseCases.Order;

public class OrderCreateCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IDreamRepository> _dreamRepositoryMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IHttpContextService> _httpContextServiceMock;
    private readonly Mock<ICacheService<string, IEnumerable<OrderResponseDto>>> _cacheServiceMock;
    private readonly OrderCreateCommandHandler _handler;

    public OrderCreateCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _dreamRepositoryMock = new Mock<IDreamRepository>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _httpContextServiceMock = new Mock<IHttpContextService>();
        _cacheServiceMock = new Mock<ICacheService<string, IEnumerable<OrderResponseDto>>>();

        _unitOfWorkMock.Setup(u => u.DreamRepository).Returns(_dreamRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.OrderRepository).Returns(_orderRepositoryMock.Object);

        _handler = new OrderCreateCommandHandler(
            _unitOfWorkMock.Object,
            _httpContextServiceMock.Object,
            _cacheServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateOrder()
    {
        // Arrange
        var faker = new Faker();
        var userId = faker.Random.Guid();
        var dreamIds = new List<Guid> { faker.Random.Guid(), faker.Random.Guid() };
        var createDto = new OrderCreateDto { DreamIds = dreamIds };
        var command = new OrderCreateCommand(createDto);
        var dreams = dreamIds.Select(id => new Dream { DreamId = id, Title = faker.Lorem.Sentence(), Description = faker.Lorem.Paragraph(), ImageFileName = "test.jpg" }).ToList();
        var orderId = faker.Random.Guid();

        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _dreamRepositoryMock.Setup(r => r.GetRangeAsync(dreamIds, CancellationToken.None)).Returns(Task.FromResult(dreams.AsQueryable()));
        _orderRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entity.Order>(), CancellationToken.None)).ReturnsAsync(orderId);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(CancellationToken.None)).Returns(Task.FromResult(1));
        _cacheServiceMock.Setup(c => c.RemoveAsync(userId.ToString() + nameof(Domain.Entity.Order))).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(orderId);
        _orderRepositoryMock.Verify(r => r.AddAsync(It.Is<Domain.Entity.Order>(o => o.UserId == userId && o.OrderDreams.Count() == 2), CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
        _cacheServiceMock.Verify(c => c.RemoveAsync(userId.ToString() + nameof(Domain.Entity.Order)), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenDreamsDoNotExist()
    {
        // Arrange
        var faker = new Faker();
        var userId = faker.Random.Guid();
        var dreamIds = new List<Guid> { faker.Random.Guid(), faker.Random.Guid() };
        var createDto = new OrderCreateDto { DreamIds = dreamIds };
        var command = new OrderCreateCommand(createDto);

        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _dreamRepositoryMock.Setup(r => r.GetRangeAsync(dreamIds, CancellationToken.None)).Returns(Task.FromResult(new List<Dream>().AsQueryable()));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var faker = new Faker();
        var dreamIds = new List<Guid> { faker.Random.Guid(), faker.Random.Guid() };
        var createDto = new OrderCreateDto { DreamIds = dreamIds };
        var command = new OrderCreateCommand(createDto);
        var dreams = dreamIds.Select(id => new Dream { DreamId = id, Title = faker.Lorem.Sentence(), Description = faker.Lorem.Paragraph(), ImageFileName = "test.jpg" }).ToList();

        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns((Guid?)null);
        _dreamRepositoryMock.Setup(r => r.GetRangeAsync(dreamIds, CancellationToken.None)).Returns(Task.FromResult(dreams.AsQueryable()));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}