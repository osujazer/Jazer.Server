using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jazer.Server.Entities;

[Table("user_peak_ranks")]
public class UserPeakRank
{
    [Key]
    [Column("user_id")]
    public required int UserId { get; init; }
    
    [Column("rank")]
    public required int Rank { get; init; }
    
    [Column("achieved_at")]
    public required DateTimeOffset AchievedAt { get; init; }

    public User User { get; init; } = null!;
}