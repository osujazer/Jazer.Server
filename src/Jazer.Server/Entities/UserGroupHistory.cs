using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Jazer.Server.Entities;

[Table("user_group_history")]
[Index(nameof(UserId))]
public class UserGroupHistory
{
    [Key]
    [Column("id")]
    public required Guid Id { get; init; }
    
    [Column("user_id")]
    public required int UserId { get; init; }
    
    [Column("group_id")]
    public required int GroupId { get; init; }
    
    [Column("created_at")]
    public required DateTimeOffset CreatedAt { get; init; }
    
    [Column("assigned_by_user_id")]
    public required int AssignedByUserId { get; init; }
    
    [Column("assign_type")]
    public required AssignType AssignType { get; init; }

    public User User { get; init; } = null!;
    
    public Group Group { get; init; } = null!;

    public User AssignedByUser { get; init; } = null!;
}

public enum AssignType : byte
{
    Add,
    Remove
}