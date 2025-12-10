using Application.UseCases.Dreams.DreamCreate;
using Application.DTO;
using Application.Exceptions;
using AutoMapper;
using Bogus;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IService;
using Domain.Model;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Shared.Configuration;
using System.IO;

namespace Tests.UnitTests.UseCases.Dreams;

public class DreamCreateCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IDreamRepository> _dreamRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IHttpContextService> _httpContextServiceMock;
    private readonly Mock<IFileStorageService> _fileStorageServiceMock;
    private readonly Mock<ICacheService<DreamCacheKey, List<DreamResponseDto>>> _cacheServiceMock;
    private readonly Mock<IOptions<BaseDreamImageConfiguration>> _baseDreamImageConfigurationMock;
    private readonly DreamCreateCommandHandler _handler;

    public DreamCreateCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _dreamRepositoryMock = new Mock<IDreamRepository>();
        _mapperMock = new Mock<IMapper>();
        _httpContextServiceMock = new Mock<IHttpContextService>();
        _fileStorageServiceMock = new Mock<IFileStorageService>();
        _cacheServiceMock = new Mock<ICacheService<DreamCacheKey, List<DreamResponseDto>>>();
        _baseDreamImageConfigurationMock = new Mock<IOptions<BaseDreamImageConfiguration>>();
        
        _unitOfWorkMock.Setup(u => u.DreamRepository).Returns(_dreamRepositoryMock.Object);
        
        _handler = new DreamCreateCommandHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _httpContextServiceMock.Object,
            _fileStorageServiceMock.Object,
            _cacheServiceMock.Object,
            _baseDreamImageConfigurationMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateDreamAndUploadImage()
    {
        // Arrange
        var faker = new Faker();
        var userId = faker.Random.Guid();
        var image = new FileModel
        {
            Content = new MemoryStream(faker.Random.Bytes(100)),
            FileName = "test.jpg",
            ContentType = "image/jpeg"
        };
        var command = new DreamCreateCommand(
            faker.Lorem.Sentence(),
            faker.Lorem.Paragraph(),
            userId,
            faker.Random.Decimal(1, 5),
            image
        );
        var dream = new Dream
        {
            DreamId = faker.Random.Guid(),
            Title = command.Title,
            Description = command.Description,
            ProducerId = userId,
            ImageFileName = ""
        };
        var objectName = faker.Random.AlphaNumeric(10);
        
        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _mapperMock.Setup(m => m.Map<Dream>(command)).Returns(dream);
        _fileStorageServiceMock.Setup(s => s.UploadFileAsync(image, CancellationToken.None)).ReturnsAsync(objectName);
        _dreamRepositoryMock.Setup(r => r.AddAsync(dream, CancellationToken.None)).ReturnsAsync(dream.DreamId);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(CancellationToken.None)).Returns(Task.FromResult(1));
        _cacheServiceMock.Setup(c => c.RemoveAsync(It.IsAny<DreamCacheKey>())).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(dream.DreamId);
        dream.ImageFileName.Should().Be(objectName);
        _httpContextServiceMock.Verify(s => s.GetCurrentUserId(), Times.Once);
        _mapperMock.Verify(m => m.Map<Dream>(command), Times.Once);
        _fileStorageServiceMock.Verify(s => s.UploadFileAsync(image, CancellationToken.None), Times.Once);
        _dreamRepositoryMock.Verify(r => r.AddAsync(dream, CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
        _cacheServiceMock.Verify(c => c.RemoveAsync(It.IsAny<DreamCacheKey>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldCreateDreamWithDefaultImage_WhenImageIsNotProvided()
    {
        // Arrange
        var faker = new Faker();
        var userId = faker.Random.Guid();
        var command = new DreamCreateCommand(
            faker.Lorem.Sentence(),
            faker.Lorem.Paragraph(),
            userId,
            faker.Random.Decimal(1, 5),
            null
        );
        var dream = new Dream
        {
            DreamId = faker.Random.Guid(),
            Title = command.Title,
            Description = command.Description,
            ProducerId = userId,
            ImageFileName = ""
        };
        var defaultImage = "default.jpg";

        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);
        _mapperMock.Setup(m => m.Map<Dream>(command)).Returns(dream);
        _baseDreamImageConfigurationMock.Setup(c => c.Value).Returns(new BaseDreamImageConfiguration { DefaultDreamImage = defaultImage });
        _dreamRepositoryMock.Setup(r => r.AddAsync(dream, CancellationToken.None)).ReturnsAsync(dream.DreamId);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(CancellationToken.None)).Returns(Task.FromResult(1));
        _cacheServiceMock.Setup(c => c.RemoveAsync(It.IsAny<DreamCacheKey>())).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(dream.DreamId);
        dream.ImageFileName.Should().Be(defaultImage);
        _httpContextServiceMock.Verify(s => s.GetCurrentUserId(), Times.Once);
        _mapperMock.Verify(m => m.Map<Dream>(command), Times.Once);
        _fileStorageServiceMock.Verify(s => s.UploadFileAsync(It.IsAny<FileModel>(), CancellationToken.None), Times.Never);
        _dreamRepositoryMock.Verify(r => r.AddAsync(dream, CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
        _cacheServiceMock.Verify(c => c.RemoveAsync(It.IsAny<DreamCacheKey>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedException_WhenUserIdIsNull()
    {
        // Arrange
        var faker = new Faker();
        var command = new DreamCreateCommand(
            faker.Lorem.Sentence(),
            faker.Lorem.Paragraph(),
            null,
            faker.Random.Decimal(1, 5),
            null
        );
        
        _httpContextServiceMock.Setup(s => s.GetCurrentUserId()).Returns((Guid?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
    }
}
