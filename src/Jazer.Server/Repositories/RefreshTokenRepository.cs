using Jazer.Server.Database;
using Jazer.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jazer.Server.Repositories;


internal sealed class RefreshTokenRepository(JazerDbContext dbContext) : IRefreshTokenRepository
{
    public async Task Add(string token, int userId, DateTimeOffset expiresAt, CancellationToken cancellationToken = default)
    {
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = token,
            UserId = userId,
            ExpiresAt = expiresAt,
        };
        
        dbContext.RefreshTokens.Add(refreshToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<RefreshToken?> FindByToken(string token, CancellationToken cancellationToken = default)
    {
        return dbContext.RefreshTokens
            .AsNoTracking()
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.Token == token, cancellationToken);
    }

    public Task UpdateRefreshToken(Guid id, string token, DateTimeOffset expiresAt, CancellationToken cancellationToken = default)
    {
        return dbContext.RefreshTokens
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(
                x => x
                    .SetProperty(y => y.Token, token)
                    .SetProperty(y => y.ExpiresAt, expiresAt),
                cancellationToken);
    }
}