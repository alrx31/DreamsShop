using Application.UseCases.Dreams.DreamGetAll;
using Application.DTO;
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

public class DreamGetAllCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IDreamRepository> _dreamRepositoryMock;
    private readonly Mock<IDreamCategoryRepository> _dreamCategoryRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IFileStorageService> _fileStorageServiceMock;
    private readonly Mock<ICacheService<DreamCacheKey, List<DreamResponseDto>>> _cacheServiceMock;
    private readonly DreamGetAllCommandHandler _handler;

    public DreamGetAllCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _dreamRepositoryMock = new Mock<IDreamRepository>();
        _dreamCategoryRepositoryMock = new Mock<IDreamCategoryRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _fileStorageServiceMock = new Mock<IFileStorageService>();
        _cacheServiceMock = new Mock<ICacheService<DreamCacheKey, List<DreamResponseDto>>>();
        
        _unitOfWorkMock.Setup(u => u.DreamRepository).Returns(_dreamRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.DreamCategoryRepository).Returns(_dreamCategoryRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.CategoryRepository).Returns(_categoryRepositoryMock.Object);
        
        _handler = new DreamGetAllCommandHandler(
            _unitOfWorkMock.Object,
            _fileStorageServiceMock.Object,
            _cacheServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnDreamsFromCache_WhenDreamsExistInCache()
    {
        // Arrange
        var faker = new Faker();
        var command = new DreamGetAllCommand { StartIndex = DreamCacheKey.DefaultStartIndex, Count = DreamCacheKey.DefaultCount };
        var cacheKey = new DreamCacheKey
        {
            StartIndex = command.StartIndex,
            Count = command.Count
        };
        var cachedDreams = new List<DreamResponseDto>
        {
            new() { Id = faker.Random.Guid(), Title = faker.Lorem.Sentence(), Description = faker.Lorem.Paragraph() },
            new() { Id = faker.Random.Guid(), Title = faker.Lorem.Sentence(), Description = faker.Lorem.Paragraph() }
        };

        _cacheServiceMock.Setup(c => c.GetAsync(It.IsAny<DreamCacheKey>())).ReturnsAsync(cachedDreams);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(cachedDreams);
        _cacheServiceMock.Verify(c => c.GetAsync(It.IsAny<DreamCacheKey>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnDreamsFromRepositoryAndSetCache_WhenDreamsDoNotExistInCache()
    {
        // Arrange
        var faker = new Faker();
        var command = new DreamGetAllCommand { StartIndex = DreamCacheKey.DefaultStartIndex, Count = DreamCacheKey.DefaultCount };
        var cacheKey = new DreamCacheKey { StartIndex = command.StartIndex, Count = command.Count };
        var dreams = new List<Dream>
        {
            new() { DreamId = faker.Random.Guid(), Title = faker.Lorem.Sentence(), Description = faker.Lorem.Paragraph(), ImageFileName = "test1.jpg" },
            new() { DreamId = faker.Random.Guid(), Title = faker.Lorem.Sentence(), Description = faker.Lorem.Paragraph(), ImageFileName = "test2.jpg" }
        };
        var imageContent1 = new MemoryStream(Encoding.UTF8.GetBytes("test image 1"));
        var imageContent2 = new MemoryStream(Encoding.UTF8.GetBytes("test image 2"));

        _cacheServiceMock.Setup(c => c.GetAsync(It.IsAny<DreamCacheKey>())).ReturnsAsync((List<DreamResponseDto>)null);
        _dreamRepositoryMock.Setup(r => r.GetRangeAsync(command.StartIndex, command.Count, CancellationToken.None)).Returns(Task.FromResult(dreams.AsQueryable()));
        _fileStorageServiceMock.Setup(s => s.DownloadFileAsync("test1.jpg", CancellationToken.None)).ReturnsAsync(new FileModel { Content = imageContent1, ContentType = "image/jpeg" });
        _fileStorageServiceMock.Setup(s => s.DownloadFileAsync("test2.jpg", CancellationToken.None)).ReturnsAsync(new FileModel { Content = imageContent2, ContentType = "image/jpeg" });
        _dreamCategoryRepositoryMock.Setup(r => r.GetCategoriesByDreamIdAsync(It.IsAny<Guid>(), CancellationToken.None)).Returns(Task.FromResult(new List<DreamCategory>().AsQueryable()));
        _categoryRepositoryMock.Setup(r => r.GetAllAsync(CancellationToken.None)).ReturnsAsync(new List<Domain.Entity.Category>().AsQueryable());
        _cacheServiceMock.Setup(c => c.SetAsync(It.IsAny<DreamCacheKey>(), It.IsAny<List<DreamResponseDto>>())).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<DreamCacheKey>(), It.IsAny<List<DreamResponseDto>>()), Times.Once);
    }
}
