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
}