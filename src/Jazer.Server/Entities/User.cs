using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Jazer.Server.Entities;

[Table("users")]
[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; init; }

    [Column("username")]
    [MinLength(3)]
    [MaxLength(16)]
    public required string Username { get; init; }
    
    [Column("email")]
    [MaxLength(90)]
    public required string Email { get; init; }
    
    [Column("hashed_password")]
    [MaxLength(120)]
    public required string HashedPassword { get; init; }
    
    [Column("created_at")]
    public required DateTimeOffset CreatedAt { get; init; }

    public IReadOnlyCollection<RefreshToken> RefreshTokens { get; init; } = null!;
    
    public IReadOnlyCollection<UserGroup> UserGroups { get; init; } = null!;
    
    public IReadOnlyCollection<UserGroupHistory> UserGroupHistories { get; init; } = null!;
    
    public IReadOnlyCollection<UserGroupHistory> UserGroupHistoriesAssigned { get; init; } = null!;
}