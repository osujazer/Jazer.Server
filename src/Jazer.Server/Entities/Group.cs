using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Jazer.Server.Entities;

[Table("groups")]
[Index(nameof(Name), IsUnique = true)]
public class Group
{
    [Key]
    [Column("id")]
    public required int Id { get; init; }
    
    [Column("name")]
    [MaxLength(30)]
    public required string Name { get; init; }
    
    [Column("description")]
    [MaxLength(100)]
    public required string Description { get; init; }

    public IReadOnlyCollection<UserGroup> UserGroups { get; init; } = null!;
    
    public IReadOnlyCollection<UserGroupHistory> UserGroupHistories { get; init; } = null!;
}