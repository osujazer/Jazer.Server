using Genbox.SimpleS3.Wasabi;
using Jazer.Server.Config;
using Microsoft.Extensions.Options;

namespace Jazer.Server.Services;

public sealed class WasabiStorageService(WasabiClient wasabiClient, IOptions<Settings> options) : IStorageService
{
    private readonly Settings _settings = options.Value;

    public async Task UploadFile(string key, byte[] fileData, CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream(fileData);
        await wasabiClient.PutObjectAsync(_settings.S3BucketName, key, memoryStream, token: cancellationToken);
    }

    public async Task<byte[]?> DownloadFile(string key, CancellationToken cancellationToken = default)
    {
        using var response = await wasabiClient.GetObjectAsync(_settings.S3BucketName, key, token: cancellationToken);

        if (!response.IsSuccess)
            return null;
        
        using var memoryStream = new MemoryStream();
        await response.Content.CopyToAsync(memoryStream, cancellationToken);
        
        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream.ToArray();
    }

    public Task DeleteFile(string key, CancellationToken cancellationToken = default)
        => wasabiClient.DeleteObjectAsync(_settings.S3BucketName, key, token: cancellationToken);
}