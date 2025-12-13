using System;
using Application.UseCases.Dreams.DreamUpdate;
using Application.DTO;
using Application.Exceptions;
using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IService;
using Domain.Model;
using FluentAssertions;
using Moq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Tests.TestHelpers;

namespace Tests.UnitTests.UseCases.Dreams;

public class DreamUpdateCommandHandlerTests : IDisposable
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IDreamRepository> _dreamRepositoryMock;
    private readonly Mock<IHttpContextService> _httpContextServiceMock;
    private readonly Mock<IFileStorageService> _fileStorageServiceMock;
    private readonly Mock<ICacheService<string, DreamResponseDto>> _cacheServiceMock;
    private readonly Mock<ICacheService<DreamCacheKey, List<DreamResponseDto>>> _allDreamCacheServiceMock;
    private readonly DreamUpdateCommandHandler _handler;
    private readonly ServiceLocatorTestHelper.ServiceLocatorTestScope _serviceScope;

    public DreamUpdateCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _dreamRepositoryMock = new Mock<IDreamRepository>();
        _httpContextServiceMock = new Mock<IHttpContextService>();
        _fileStorageServiceMock = new Mock<IFileStorageService>();
        _cacheServiceMock = new Mock<ICacheService<string, DreamResponseDto>>();
        _allDreamCacheServiceMock = new Mock<ICacheService<DreamCacheKey, List<DreamResponseDto>>>();
        
        _unitOfWorkMock.Setup(u => u.DreamRepository).Returns(_dreamRepositoryMock.Object);

        _serviceScope = ServiceLocatorTestHelper.UseServiceLocator(
            (typeof(IUnitOfWork), _unitOfWorkMock.Object));
        
        _handler = new DreamUpdateCommandHandler(
            _httpContextServiceMock.Object,
            _fileStorageServiceMock.Object,
            _cacheServiceMock.Object,
            _allDreamCacheServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldUpdateDream()
    {
        // Arrange
        var faker = new Faker();
        var dreamId = faker.Random.Guid();
        var userId = faker.Random.Guid();
        var updateDto = new DreamUpdateDto
        {
            Title = faker.Lorem.Sentence(),
            Description = faker.Lorem.Paragraph(),
            Image = new FileModel
            {
                Content = new MemoryStream(faker.Random.Bytes(100)),
                FileName = "test.jpg",
                ContentType = "image/jpeg"
            }
        };
        var command = new DreamUpdateCommand(dreamId, updateDto);
        var dream = new Dream
        {
            DreamId = dreamId,
            Title = faker.Lorem.Sentence(),
            Description = faker.Lorem.Paragraph(),
            ProducerId = userId,
            ImageFileName = faker.System.FileName()
        };
        var objectName = faker.Random.AlphaNumeric(10);

        _dreamRepositoryMock.Setup(r => r.GetAsync(new [] {dreamId}, CancellationToken.None)).ReturnsAsync(dream);
        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _fileStorageServiceMock.Setup(s => s.UploadFileAsync(updateDto.Image, CancellationToken.None)).ReturnsAsync(objectName);
        _allDreamCacheServiceMock.Setup(c => c.RemoveAsync(It.IsAny<DreamCacheKey>())).Returns(Task.CompletedTask);
        _cacheServiceMock.Setup(c => c.RemoveAsync(dreamId.ToString() + nameof(Dream))).Returns(Task.CompletedTask);
        _dreamRepositoryMock.Setup(r => r.UpdateAsync(dream, CancellationToken.None)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(CancellationToken.None)).Returns(Task.FromResult(1));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        dream.Title.Should().Be(updateDto.Title);
        dream.Description.Should().Be(updateDto.Description);
        dream.ImageFileName.Should().Be(objectName);
        _dreamRepositoryMock.Verify(r => r.GetAsync(new [] {dreamId}, CancellationToken.None), Times.Once);
        _httpContextServiceMock.Verify(s => s.GetCurrentUserId(), Times.Once);
        _fileStorageServiceMock.Verify(s => s.UploadFileAsync(updateDto.Image, CancellationToken.None), Times.Once);
        _allDreamCacheServiceMock.Verify(c => c.RemoveAsync(It.IsAny<DreamCacheKey>()), Times.Once);
        _cacheServiceMock.Verify(c => c.RemoveAsync(dreamId.ToString() + nameof(Dream)), Times.Once);
        _dreamRepositoryMock.Verify(r => r.UpdateAsync(dream, CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenDreamDoesNotExist()
    {
        // Arrange
        var dreamId = Guid.NewGuid();
        var updateDto = new DreamUpdateDto();
        var command = new DreamUpdateCommand(dreamId, updateDto);

        _dreamRepositoryMock.Setup(r => r.GetAsync(new [] {dreamId}, CancellationToken.None)).ReturnsAsync((Dream)null!);

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
        var dreamId = faker.Random.Guid();
        var userId = faker.Random.Guid();
        var otherUserId = faker.Random.Guid();
        var updateDto = new DreamUpdateDto();
        var command = new DreamUpdateCommand(dreamId, updateDto);
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
        await act.Should().ThrowAsync<ForbiddenException>();
    }

    public void Dispose()
    {
        _serviceScope.Dispose();
        GC.SuppressFinalize(this);
    }
}
