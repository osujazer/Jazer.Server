using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Jazer.Server.Entities;

[Table("refresh_tokens")]
[Index(nameof(Token), IsUnique = true)]
public class RefreshToken
{
    [Column("id")]
    [Key]
    public required Guid Id { get; init; }
    
    [Column("token")]
    [MaxLength(200)]
    public required string Token { get; init; }
    
    [Column("user_id")]
    public required int UserId { get; init; }
    
    [Column("expires_at")]
    public required DateTimeOffset ExpiresAt { get; init; }

    public User User { get; init; } = null!;
}