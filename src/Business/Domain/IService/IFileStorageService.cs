using Domain.Model;

namespace Domain.IService;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(FileModel file, CancellationToken cancellationToken = default);
    Task<FileModel> DownloadFileAsync(string fileName, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(string fileName, CancellationToken cancellationToken = default);
}