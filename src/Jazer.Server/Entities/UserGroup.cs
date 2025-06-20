using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Jazer.Server.Entities;

[Table("user_groups")]
[PrimaryKey(nameof(UserId), nameof(GroupId))]
public class UserGroup
{
    [Column("user_id")]
    public required int UserId { get; init; }
    
    [Column("group_id")]
    public required int GroupId { get; init; }

    public User User { get; init; } = null!;

    public Group Group { get; init; } = null!;
}