namespace Jazer.Server.Services;

public class AvatarService(IStorageService storageService) : IAvatarService
{
    private const string DefaultAvatarKey = "avatars/default.png";

    private static string AvatarKey(int userId) => $"avatars/{userId}.png";

    public async Task SetAvatar(int userId, byte[] avatarData, CancellationToken cancellationToken)
    {
        var avatarKey = AvatarKey(userId);
        
        await storageService.UploadFile(avatarKey, avatarData, cancellationToken);
    }

    public async Task<byte[]> GetAvatar(int userId, CancellationToken cancellationToken)
    {
        var avatarKey = AvatarKey(userId);
        
        var userAvatar = await storageService.DownloadFile(avatarKey, cancellationToken);

        if (userAvatar is not null)
            return userAvatar;
        
        var defaultAvatar = await storageService.DownloadFile(DefaultAvatarKey, cancellationToken);

        return defaultAvatar!;
    }

    public Task DeleteAvatar(int userId, CancellationToken cancellationToken)
    {
        var avatarKey = AvatarKey(userId);
        
        return storageService.DeleteFile(avatarKey, cancellationToken);
    }
}