using Jazer.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jazer.Server.Database;

public class JazerDbContext(DbContextOptions<JazerDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }
    
    public DbSet<RefreshToken> RefreshTokens { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>()
            .HasOne(refreshToken => refreshToken.User)
            .WithMany(user => user.RefreshTokens)
            .HasPrincipalKey(user => user.Id)
            .HasForeignKey(refreshToken => refreshToken.UserId);
    }
}