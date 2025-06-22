using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jazer.Server.Entities;

[Table("user_statistics")]
public class UserStatistics
{
    [Key]
    [Column("user_id")]
    public required int UserId { get; init; }
    
    [Column("jazer_points")]
    public required int JazerPoints { get; init; }
    
    [Column("ranked_score")]
    public required long RankedScore { get; init; }
    
    [Column("total_score")]
    public required long TotalScore { get; init; }
    
    [Column("average_accuracy")]
    public required double AverageAccuracy { get; init; }
    
    [Column("global_rank")]
    public int? GlobalRank { get; init; }
    
    [Column("country_rank")]
    public int? CountryRank { get; init; }
    
    [Column("play_time")]
    public required long PlayTimeSeconds { get; init; }
    
    [Column("play_count")]
    public required int PlayCount { get; init; }
    
    [Column("max_combo")]
    public required int MaxCombo { get; init; }
    
    [Column("total_hits")]
    public required long TotalHits { get; init; }

    public User User { get; init; } = null!;
}