namespace Jazer.Server.Services;

public interface IAvatarService
{
    Task SetAvatar(int userId, byte[] avatarData, CancellationToken cancellationToken);
    
    Task<byte[]> GetAvatar(int userId, CancellationToken cancellationToken);
    
    Task DeleteAvatar(int userId, CancellationToken cancellationToken);
}