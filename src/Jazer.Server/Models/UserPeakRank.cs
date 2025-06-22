using System.Text.Json.Serialization;

namespace Jazer.Server.Models;

public class UserPeakRank
{
    [JsonPropertyName("rank")]
    public required int Rank { get; init; }
    
    [JsonPropertyName("achieved_at")]
    public required DateTimeOffset AchievedAt { get; init; }
}