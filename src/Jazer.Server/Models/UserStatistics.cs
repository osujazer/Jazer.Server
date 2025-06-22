using System.Text.Json.Serialization;

namespace Jazer.Server.Models;

public class UserStatistics
{
    [JsonPropertyName("jazer_points")]
    public required int JazerPoints { get; init; }
    
    [JsonPropertyName("ranked_score")]
    public required long RankedScore { get; init; }
    
    [JsonPropertyName("total_score")]
    public required long TotalScore { get; init; }
    
    [JsonPropertyName("average_accuracy")]
    public required double AverageAccuracy { get; init; }
    
    [JsonPropertyName("global_rank")]
    public int? GlobalRank { get; init; }
    
    [JsonPropertyName("country_rank")]
    public int? CountryRank { get; init; }
    
    [JsonPropertyName("play_time")]
    public required long PlayTimeSeconds { get; init; }
    
    [JsonPropertyName("play_count")]
    public required int PlayCount { get; init; }
    
    [JsonPropertyName("max_combo")]
    public required int MaxCombo { get; init; }
    
    [JsonPropertyName("total_hits")]
    public required long TotalHits { get; init; }
}