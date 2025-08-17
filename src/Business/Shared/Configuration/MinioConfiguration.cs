namespace Shared.Configuration;

public class MinioConfiguration
{
    public required string Endpoint { get; init; }
    public required int Port { get; init; }
    public required string AccessKey { get; init; }
    public required string SecretKey { get; init; }
    public required string BucketName { get; init; }
}