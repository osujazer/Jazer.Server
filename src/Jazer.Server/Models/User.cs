using System.Text.Json.Serialization;

namespace Jazer.Server.Models;

public class User
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }
    
    [JsonPropertyName("username")]
    public required string Username { get; init; }
    
    [JsonPropertyName("created_at")]
    public required DateTimeOffset CreatedAt { get; init; }
    
    [JsonPropertyName("country_code")]
    public required string CountryCode { get; init; }
    
    [JsonPropertyName("statistics")]
    public required UserStatistics Statistics { get; init; }
    
    [JsonPropertyName("peak_rank")]
    public UserPeakRank? PeakRank { get; init; }
}