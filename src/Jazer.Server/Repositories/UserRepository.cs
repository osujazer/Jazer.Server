using Jazer.Server.Database;
using Jazer.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jazer.Server.Repositories;

public sealed class UserRepository(JazerDbContext dbContext) : IUserRepository
{
    public async Task<int> Add(string username, string email, string hashedPassword, CancellationToken cancellationToken = default)
    {
        var user = new User
        {
            Username = username,
            Email = email,
            HashedPassword = hashedPassword,
            CreatedAt = DateTimeOffset.UtcNow,
        };
        
        var entityEntry = dbContext.Users.Add(user);

        await dbContext.SaveChangesAsync(cancellationToken);

        return entityEntry.Entity.Id;
    }

    public Task<bool> UsernameExists(string username, CancellationToken cancellationToken = default)
    {
        return dbContext.Users
            .AsNoTracking()
            .AnyAsync(x => EF.Functions.ILike(x.Username,username), cancellationToken);
    }

    public Task<bool> EmailExists(string email, CancellationToken cancellationToken = default)
    {
        return dbContext.Users
            .AsNoTracking()
            .AnyAsync(x => EF.Functions.ILike(x.Email,email), cancellationToken);
    }
}