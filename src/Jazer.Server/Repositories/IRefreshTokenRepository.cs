using Jazer.Server.Entities;

namespace Jazer.Server.Repositories;

public interface IRefreshTokenRepository
{
    Task Add(string token, int userId, DateTimeOffset expiresAt, CancellationToken cancellationToken = default);
    
    Task<RefreshToken?> FindByToken(string token, CancellationToken cancellationToken = default);
    
    Task UpdateRefreshToken(Guid id, string token, DateTimeOffset expiresAt, CancellationToken cancellationToken = default);
}