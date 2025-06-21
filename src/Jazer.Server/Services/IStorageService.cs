namespace Jazer.Server.Services;

public interface IStorageService
{
    Task UploadFile(string key, byte[] fileData, CancellationToken cancellationToken = default);

    Task<byte[]?> DownloadFile(string key, CancellationToken cancellationToken = default);
    
    Task DeleteFile(string key, CancellationToken cancellationToken = default);
}