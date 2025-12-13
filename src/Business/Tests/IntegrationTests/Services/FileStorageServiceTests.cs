using Bogus;
using Domain.Model;
using Infrastructure.Services;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using System.Text;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using Minio;
using Minio.DataModel.Args;

namespace Tests.IntegrationTests.Services;

public class FileStorageServiceTests : IAsyncLifetime
{
    private readonly IContainer _minioContainer;
    private FileStorageService _fileStorageService = null!;
    private MinioConfiguration _minioConfiguration = null!;

    public FileStorageServiceTests()
    {
        _minioContainer = new ContainerBuilder()
            .WithImage("minio/minio")
            .WithPortBinding(9000, true)
            .WithPortBinding(9001, true)
            .WithCommand("server", "/data", "--console-address", ":9001")
            .WithEnvironment("MINIO_ROOT_USER", "minioadmin")
            .WithEnvironment("MINIO_ROOT_PASSWORD", "minioadmin")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _minioContainer.StartAsync();
        _minioConfiguration = new MinioConfiguration
        {
            Endpoint = _minioContainer.Hostname,
            Port = _minioContainer.GetMappedPublicPort(9000),
            AccessKey = "minioadmin",
            SecretKey = "minioadmin",
            BucketName = "test-bucket"
        };
        var options = Options.Create(_minioConfiguration);
        _fileStorageService = new FileStorageService(options);
        
        var minioClient = new MinioClient()
            .WithEndpoint(_minioConfiguration.Endpoint, _minioConfiguration.Port)
            .WithCredentials(_minioConfiguration.AccessKey, _minioConfiguration.SecretKey)
            .Build();
        
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(_minioConfiguration.BucketName);
        var found = await minioClient.BucketExistsAsync(bucketExistsArgs);
        if (!found)
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(_minioConfiguration.BucketName);
            await minioClient.MakeBucketAsync(makeBucketArgs);
        }
    }

    public Task DisposeAsync()
    {
        return _minioContainer.DisposeAsync().AsTask();
    }

    [Fact]
    public async Task UploadFileAsync_ShouldUploadFile()
    {
        // Arrange
        var faker = new Faker();
        var fileContent = new MemoryStream(Encoding.UTF8.GetBytes(faker.Lorem.Paragraph()));
        var file = new FileModel
        {
            Content = fileContent,
            FileName = "test.txt",
            ContentType = "text/plain"
        };

        // Act
        var fileName = await _fileStorageService.UploadFileAsync(file);

        // Assert
        fileName.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task DownloadFileAsync_ShouldDownloadFile()
    {
        // Arrange
        var faker = new Faker();
        var fileContent = new MemoryStream(Encoding.UTF8.GetBytes(faker.Lorem.Paragraph()));
        var file = new FileModel
        {
            Content = fileContent,
            FileName = "test.txt",
            ContentType = "text/plain"
        };
        var fileName = await _fileStorageService.UploadFileAsync(file);

        // Act
        var downloadedFile = await _fileStorageService.DownloadFileAsync(fileName);

        // Assert
        downloadedFile.Should().NotBeNull();
    }
}
