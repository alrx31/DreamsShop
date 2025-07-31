namespace Domain.Model;

public class FileModel
{
    public string? FileName { get; init; }
    public string? ContentType { get; init; }
    public Stream? Content { get; init; }
}