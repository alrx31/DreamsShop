using Application.UseCases.Order.OrderGetAllByUser;
using Application.DTO.Order;
using Application.Exceptions;
using AutoMapper;
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

public class OrderGetAllByUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IHttpContextService> _httpContextServiceMock;
    private readonly Mock<ICacheService<string, IEnumerable<OrderResponseDto>>> _cacheServiceMock;
    private readonly OrderGetAllByUserCommandHandler _handler;

    public OrderGetAllByUserCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _mapperMock = new Mock<IMapper>();
        _httpContextServiceMock = new Mock<IHttpContextService>();
        _cacheServiceMock = new Mock<ICacheService<string, IEnumerable<OrderResponseDto>>>();
        
        _unitOfWorkMock.Setup(u => u.OrderRepository).Returns(_orderRepositoryMock.Object);
        
        _handler = new OrderGetAllByUserCommandHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _httpContextServiceMock.Object,
            _cacheServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnOrdersFromCache_WhenOrdersExistInCache()
    {
        // Arrange
        var faker = new Faker();
        var userId = faker.Random.Guid();
        var command = new OrderGetAllByUserCommand(0, 10);
        var cachedOrders = new List<OrderResponseDto>
        {
            new() { OrderId = faker.Random.Guid() },
            new() { OrderId = faker.Random.Guid() }
        };

        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _cacheServiceMock.Setup(c => c.GetAsync(userId.ToString() + nameof(Domain.Entity.Order))).ReturnsAsync(cachedOrders);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(cachedOrders);
        _cacheServiceMock.Verify(c => c.GetAsync(userId.ToString() + nameof(Domain.Entity.Order)), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnOrdersFromRepositoryAndSetCache_WhenOrdersDoNotExistInCache()
    {
        // Arrange
        var faker = new Faker();
        var userId = faker.Random.Guid();
        var command = new OrderGetAllByUserCommand(0, 10);
        var orders = new List<Domain.Entity.Order>
        {
            new() { OrderId = faker.Random.Guid(), UserId = userId },
            new() { OrderId = faker.Random.Guid(), UserId = userId }
        };
        var mappedOrders = new List<OrderResponseDto>
        {
            new() { OrderId = orders[0].OrderId },
            new() { OrderId = orders[1].OrderId }
        };

        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _cacheServiceMock.Setup(c => c.GetAsync(userId.ToString() + nameof(Domain.Entity.Order))).ReturnsAsync((IEnumerable<OrderResponseDto>)null);
        _orderRepositoryMock.Setup(r => r.GetOrdersByUser(userId, command.StartIndex, command.Skip, CancellationToken.None)).Returns(Task.FromResult(orders.AsQueryable()));
        _mapperMock.Setup(m => m.Map<IEnumerable<OrderResponseDto>>(orders)).Returns(mappedOrders);
        _cacheServiceMock.Setup(c => c.SetAsync(userId.ToString() + nameof(Domain.Entity.Order), mappedOrders)).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(mappedOrders);
        _orderRepositoryMock.Verify(r => r.GetOrdersByUser(userId, command.StartIndex, command.Skip, CancellationToken.None), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(userId.ToString() + nameof(Domain.Entity.Order), mappedOrders), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedException_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var command = new OrderGetAllByUserCommand(0, 10);
        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns((Guid?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
    }
}
