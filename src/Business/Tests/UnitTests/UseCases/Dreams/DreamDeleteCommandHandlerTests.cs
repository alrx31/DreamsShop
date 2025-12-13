using System;
using Application.UseCases.Dreams.DreamDelete;
using Application.DTO;
using Application.Exceptions;
using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IService;
using Domain.Model;
using FluentAssertions;
using Moq;
using Tests.TestHelpers;

namespace Tests.UnitTests.UseCases.Dreams;

public class DreamDeleteCommandHandlerTests : IDisposable
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IDreamRepository> _dreamRepositoryMock;
    private readonly Mock<ICacheService<string, DreamResponseDto>> _cacheServiceMock;
    private readonly Mock<ICacheService<DreamCacheKey, List<DreamResponseDto>>> _allDreamCacheServiceMock;
    private readonly Mock<IHttpContextService> _httpContextServiceMock;
    private readonly DreamDeleteCommandHandler _handler;
    private readonly ServiceLocatorTestHelper.ServiceLocatorTestScope _serviceScope;

    public DreamDeleteCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _dreamRepositoryMock = new Mock<IDreamRepository>();
        _cacheServiceMock = new Mock<ICacheService<string, DreamResponseDto>>();
        _allDreamCacheServiceMock = new Mock<ICacheService<DreamCacheKey, List<DreamResponseDto>>>();
        _httpContextServiceMock = new Mock<IHttpContextService>();
        
        _unitOfWorkMock.Setup(u => u.DreamRepository).Returns(_dreamRepositoryMock.Object);

        _serviceScope = ServiceLocatorTestHelper.UseServiceLocator(
            (typeof(IUnitOfWork), _unitOfWorkMock.Object));
        
        _handler = new DreamDeleteCommandHandler(
            _cacheServiceMock.Object,
            _allDreamCacheServiceMock.Object,
            _httpContextServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldDeleteDream()
    {
        // Arrange
        var faker = new Faker();
        var dreamId = faker.Random.Guid();
        var userId = faker.Random.Guid();
        var command = new DreamDeleteCommand(dreamId);
        var dream = new Dream
        {
            DreamId = dreamId,
            Title = faker.Lorem.Sentence(),
            Description = faker.Lorem.Paragraph(),
            ProducerId = userId,
            ImageFileName = faker.System.FileName()
        };

        _dreamRepositoryMock.Setup(r => r.GetAsync(new [] {dreamId}, CancellationToken.None)).ReturnsAsync(dream);
        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _cacheServiceMock.Setup(c => c.RemoveAsync(dreamId.ToString() + nameof(Dream))).Returns(Task.CompletedTask);
        _allDreamCacheServiceMock.Setup(c => c.RemoveAsync(It.IsAny<DreamCacheKey>())).Returns(Task.CompletedTask);
        _dreamRepositoryMock.Setup(r => r.DeleteAsync(dream, CancellationToken.None)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(CancellationToken.None)).Returns(Task.FromResult(1));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _dreamRepositoryMock.Verify(r => r.GetAsync(new [] {dreamId}, CancellationToken.None), Times.Once);
        _httpContextServiceMock.Verify(s => s.GetCurrentUserId(), Times.Once);
        _cacheServiceMock.Verify(c => c.RemoveAsync(dreamId.ToString() + nameof(Dream)), Times.Once);
        _allDreamCacheServiceMock.Verify(c => c.RemoveAsync(It.IsAny<DreamCacheKey>()), Times.Once);
        _dreamRepositoryMock.Verify(r => r.DeleteAsync(dream, CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenDreamDoesNotExist()
    {
        // Arrange
        var dreamId = Guid.NewGuid();
        var command = new DreamDeleteCommand(dreamId);

        _dreamRepositoryMock.Setup(r => r.GetAsync(new [] {dreamId}, CancellationToken.None)).ReturnsAsync((Dream)null!);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
    
    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedException_WhenUserIsNotAuthorized()
    {
        // Arrange
        var faker = new Faker();
        var dreamId = faker.Random.Guid();
        var userId = faker.Random.Guid();
        var otherUserId = faker.Random.Guid();
        var command = new DreamDeleteCommand(dreamId);
        var dream = new Dream
        {
            DreamId = dreamId,
            Title = faker.Lorem.Sentence(),
            Description = faker.Lorem.Paragraph(),
            ProducerId = userId,
            ImageFileName = faker.System.FileName()
        };

        _dreamRepositoryMock.Setup(r => r.GetAsync(new [] {dreamId}, CancellationToken.None)).ReturnsAsync(dream);
        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns(otherUserId);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}
