using Jazer.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jazer.Server.Database;

public class JazerDbContext(DbContextOptions<JazerDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }
    
    public DbSet<RefreshToken> RefreshTokens { get; init; }
    
    public DbSet<Group> Groups { get; init; }
    
    public DbSet<UserGroup> UserGroups { get; init; }
    
    public DbSet<UserGroupHistory> UserGroupHistories { get; init; }

    public DbSet<UserInfraction> UserInfractions { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>()
            .HasOne(refreshToken => refreshToken.User)
            .WithMany(user => user.RefreshTokens)
            .HasPrincipalKey(user => user.Id)
            .HasForeignKey(refreshToken => refreshToken.UserId);

        modelBuilder.Entity<UserGroup>()
            .HasOne(userGroup => userGroup.User)
            .WithMany(user => user.UserGroups)
            .HasPrincipalKey(user => user.Id)
            .HasForeignKey(userGroup => userGroup.UserId);
        
        modelBuilder.Entity<UserGroupHistory>()
            .HasOne(userGroupHistory => userGroupHistory.User)
            .WithMany(user => user.UserGroupHistories)
            .HasPrincipalKey(user => user.Id)
            .HasForeignKey(userGroupHistory => userGroupHistory.UserId);
        
        modelBuilder.Entity<UserGroup>()
            .HasOne(userGroup => userGroup.Group)
            .WithMany(group => group.UserGroups)
            .HasPrincipalKey(group => group.Id)
            .HasForeignKey(userGroup => userGroup.GroupId);
        
        modelBuilder.Entity<UserGroupHistory>()
            .HasOne(userGroupHistory => userGroupHistory.Group)
            .WithMany(group => group.UserGroupHistories)
            .HasPrincipalKey(group => group.Id)
            .HasForeignKey(userGroupHistory => userGroupHistory.GroupId);
        
        modelBuilder.Entity<UserGroupHistory>()
            .HasOne(userGroupHistory => userGroupHistory.AssignedByUser)
            .WithMany(user => user.UserGroupHistoriesAssigned)
            .HasPrincipalKey(user => user.Id)
            .HasForeignKey(userGroupHistory => userGroupHistory.AssignedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<UserInfraction>()
            .HasOne(userInfraction => userInfraction.User)
            .WithMany(user => user.UserInfractions)
            .HasPrincipalKey(user => user.Id)
            .HasForeignKey(userInfraction => userInfraction.UserId);
        
        modelBuilder.Entity<UserInfraction>()
            .HasOne(userInfraction => userInfraction.AssignedByUser)
            .WithMany(user => user.UserInfractionsAssigned)
            .HasPrincipalKey(user => user.Id)
            .HasForeignKey(userInfraction => userInfraction.AssignedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}