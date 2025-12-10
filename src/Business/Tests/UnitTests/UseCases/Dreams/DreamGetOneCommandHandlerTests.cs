using Application.UseCases.Dreams.DreamsGetOne;
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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.UnitTests.UseCases.Dreams;

public class DreamGetOneCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IDreamRepository> _dreamRepositoryMock;
    private readonly Mock<IDreamCategoryRepository> _dreamCategoryRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IFileStorageService> _fileStorageServiceMock;
    private readonly Mock<ICacheService<string, DreamResponseDto>> _cacheServiceMock;
    private readonly DreamGetOneCommandHandler _handler;

    public DreamGetOneCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _dreamRepositoryMock = new Mock<IDreamRepository>();
        _dreamCategoryRepositoryMock = new Mock<IDreamCategoryRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _fileStorageServiceMock = new Mock<IFileStorageService>();
        _cacheServiceMock = new Mock<ICacheService<string, DreamResponseDto>>();
        
        _unitOfWorkMock.Setup(u => u.DreamRepository).Returns(_dreamRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.DreamCategoryRepository).Returns(_dreamCategoryRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.CategoryRepository).Returns(_categoryRepositoryMock.Object);
        
        _handler = new DreamGetOneCommandHandler(
            _unitOfWorkMock.Object,
            _fileStorageServiceMock.Object,
            _cacheServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnDreamFromCache_WhenDreamExistsInCache()
    {
        // Arrange
        var faker = new Faker();
        var dreamId = faker.Random.Guid();
        var command = new DreamGetOneCommand { DreamId = dreamId };
        var cachedDream = new DreamResponseDto
        {
            Id = dreamId,
            Title = faker.Lorem.Sentence(),
            Description = faker.Lorem.Paragraph()
        };

        _cacheServiceMock.Setup(c => c.GetAsync(dreamId.ToString() + nameof(Dream))).ReturnsAsync(cachedDream);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(cachedDream);
        _cacheServiceMock.Verify(c => c.GetAsync(dreamId.ToString() + nameof(Dream)), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnDreamFromRepositoryAndSetCache_WhenDreamDoesNotExistInCache()
    {
        // Arrange
        var faker = new Faker();
        var dreamId = faker.Random.Guid();
        var command = new DreamGetOneCommand { DreamId = dreamId };
        var dream = new Dream
        {
            DreamId = dreamId,
            Title = faker.Lorem.Sentence(),
            Description = faker.Lorem.Paragraph(),
            ImageFileName = "test.jpg"
        };
        var imageContent = new MemoryStream(Encoding.UTF8.GetBytes("test image"));

        _cacheServiceMock.Setup(c => c.GetAsync(dreamId.ToString() + nameof(Dream))).ReturnsAsync((DreamResponseDto)null);
        _dreamRepositoryMock.Setup(r => r.GetAsync(new [] {dreamId}, CancellationToken.None)).ReturnsAsync(dream);
        _fileStorageServiceMock.Setup(s => s.DownloadFileAsync("test.jpg", CancellationToken.None)).ReturnsAsync(new FileModel { Content = imageContent, ContentType = "image/jpeg" });
        _dreamCategoryRepositoryMock.Setup(r => r.GetCategoriesByDreamIdAsync(dreamId, CancellationToken.None)).Returns(Task.FromResult(new List<DreamCategory>().AsQueryable()));
        _categoryRepositoryMock.Setup(r => r.GetAllAsync(CancellationToken.None)).ReturnsAsync(new List<Domain.Entity.Category>().AsQueryable());
        _cacheServiceMock.Setup(c => c.SetAsync(dreamId.ToString() + nameof(Dream), It.IsAny<DreamResponseDto>())).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(dreamId);
        _cacheServiceMock.Verify(c => c.SetAsync(dreamId.ToString() + nameof(Dream), It.IsAny<DreamResponseDto>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenDreamDoesNotExist()
    {
        // Arrange
        var dreamId = Guid.NewGuid();
        var command = new DreamGetOneCommand { DreamId = dreamId };

        _cacheServiceMock.Setup(c => c.GetAsync(dreamId.ToString() + nameof(Dream))).ReturnsAsync((DreamResponseDto)null);
        _dreamRepositoryMock.Setup(r => r.GetAsync(new [] {dreamId}, CancellationToken.None)).ReturnsAsync((Dream)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
