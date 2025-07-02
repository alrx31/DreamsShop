using Domain.IService;
using Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Shared.Configuration;

namespace Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly MinioConfiguration _minioConfiguration;

    public FileStorageService(IOptions<MinioConfiguration> minioConfiguration)
    {
        _minioConfiguration = minioConfiguration.Value;
        _minioClient = new MinioClient()
            .WithEndpoint(_minioConfiguration.Endpoint,_minioConfiguration.Port)
            .WithCredentials(_minioConfiguration.AccessKey, _minioConfiguration.SecretKey)
            .Build();
    }


    public async Task<string> UploadFileAsync(FileModel file, CancellationToken cancellationToken = default)
    {
        var fileName = $"{Guid.NewGuid()}_{file.FileName}";

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_minioConfiguration.BucketName)
            .WithObject(fileName)
            .WithStreamData(file.Content)
            .WithObjectSize(file?.Content?.Length ?? 0)
            .WithContentType(file?.ContentType ?? "");

        await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
        
        return fileName;
    }

    public async Task<FileModel> DownloadFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var memoryStream = new MemoryStream();
        
        var statArgs = new StatObjectArgs()
            .WithBucket(_minioConfiguration.BucketName)
            .WithObject(fileName);
        var stat = await _minioClient.StatObjectAsync(statArgs, cancellationToken);
        
        var getObjectArgs = new GetObjectArgs()
            .WithBucket(_minioConfiguration.BucketName)
            .WithObject(fileName)
            .WithCallbackStream(stream => stream.CopyTo(memoryStream));
        await _minioClient.GetObjectAsync(getObjectArgs, cancellationToken);
        
        memoryStream.Position = 0;

        return new FileModel
        {
            Content = memoryStream,
            FileName = fileName,
            ContentType = stat.ContentType,
        };
    }

    public async Task DeleteFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var removeArgs = new RemoveObjectArgs()
            .WithBucket(_minioConfiguration.BucketName)
            .WithObject(fileName);
        await _minioClient.RemoveObjectAsync(removeArgs, cancellationToken);
    }
}