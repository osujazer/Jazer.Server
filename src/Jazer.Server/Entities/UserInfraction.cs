using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Jazer.Server.Entities;

[Table("user_infractions")]
[Index(nameof(UserId))]
public class UserInfraction
{
    [Key]
    [Column("id")]
    public required Guid Id { get; init; }
    
    [Column("user_id")]
    public required int UserId { get; init; }
    
    [Column("created_at")]
    public required DateTimeOffset CreatedAt { get; init; }
    
    [Column("expires_at")]
    public DateTimeOffset? ExpiresAt { get; init; }
    
    [Column("assigned_by_user_id")]
    public required int AssignedByUserId { get; init; }
    
    [Column("infraction_type")]
    public required InfractionType InfractionType { get; init; }

    [Column("infraction_reason")]
    public required InfractionReason InfractionReason { get; init; }
    
    [Column("public_detail")]
    [MaxLength(500)]
    public string? PublicDetail { get; init; }
    
    [Column("internal_detail")]
    [MaxLength(500)]
    public required string InternalDetail { get; init; }

    public User User { get; init; } = null!;
    
    public User AssignedByUser { get; init; } = null!;
}

public enum InfractionType : byte
{
    Mute,
    Ban,
}

public enum InfractionReason : byte
{
    Spam,
    Cheating,
    MultipleAccounts
}