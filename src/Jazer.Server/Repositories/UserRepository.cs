using Jazer.Server.Database;
using Jazer.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jazer.Server.Repositories;

internal sealed class UserRepository(JazerDbContext dbContext) : IUserRepository
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

    public Task<User?> FindByUsername(string username, CancellationToken cancellationToken = default)
    {
        return dbContext.Users
            .AsNoTracking()
            .Where(x => EF.Functions.ILike(x.Username, username))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<User?> FindByEmail(string email, CancellationToken cancellationToken = default)
    {
        return dbContext.Users
            .AsNoTracking()
            .Where(x => EF.Functions.ILike(x.Email, email))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<User?> FindById(int id, CancellationToken cancellationToken = default)
    {
        return dbContext.Users
            .AsNoTracking()
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync(cancellationToken);
    }
}