namespace Shared.Configuration;

public class MinioConfiguration
{
    public required string Endpoint { get; set; }
    public required int Port { get; set; }
    public required string AccessKey { get; set; }
    public required string SecretKey { get; set; }
    public required string BucketName { get; set; }
}