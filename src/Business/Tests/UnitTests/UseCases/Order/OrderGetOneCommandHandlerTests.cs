using Application.UseCases.Order.OrderGetOne;
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
using System.Threading;
using System.Threading.Tasks;

namespace Tests.UnitTests.UseCases.Order;

public class OrderGetOneCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IHttpContextService> _httpContextServiceMock;
    private readonly OrderGetOneCommandHandler _handler;

    public OrderGetOneCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _mapperMock = new Mock<IMapper>();
        _httpContextServiceMock = new Mock<IHttpContextService>();
        
        _unitOfWorkMock.Setup(u => u.OrderRepository).Returns(_orderRepositoryMock.Object);
        
        _handler = new OrderGetOneCommandHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _httpContextServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnOrder()
    {
        // Arrange
        var faker = new Faker();
        var orderId = faker.Random.Guid();
        var userId = faker.Random.Guid();
        var command = new OrderGetOneCommand(orderId);
        var order = new Domain.Entity.Order
        {
            OrderId = orderId,
            UserId = userId
        };
        var mappedOrder = new OrderResponseDto
        {
            OrderId = orderId
        };

        _orderRepositoryMock.Setup(r => r.GetAsync(new [] {orderId}, CancellationToken.None)).ReturnsAsync(order);
        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _mapperMock.Setup(m => m.Map<OrderResponseDto>(order)).Returns(mappedOrder);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(mappedOrder);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenOrderDoesNotExist()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new OrderGetOneCommand(orderId);

        _orderRepositoryMock.Setup(r => r.GetAsync(new [] {orderId}, CancellationToken.None)).ReturnsAsync((Domain.Entity.Order)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
    
    [Fact]
    public async Task Handle_ShouldThrowForbiddenException_WhenUserIsNotAuthorized()
    {
        // Arrange
        var faker = new Faker();
        var orderId = faker.Random.Guid();
        var userId = faker.Random.Guid();
        var otherUserId = faker.Random.Guid();
        var command = new OrderGetOneCommand(orderId);
        var order = new Domain.Entity.Order
        {
            OrderId = orderId,
            UserId = userId
        };

        _orderRepositoryMock.Setup(r => r.GetAsync(new [] {orderId}, CancellationToken.None)).ReturnsAsync(order);
        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns(otherUserId);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ForbiddenException>();
    }
}
