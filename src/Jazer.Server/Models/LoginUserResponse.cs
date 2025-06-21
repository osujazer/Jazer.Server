using System.Text.Json.Serialization;

namespace Jazer.Server.Models;

public class LoginUserResponse
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; init; }
    
    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; init; }
    
    [JsonPropertyName("expires_at")]
    public required DateTimeOffset ExpiresAt { get; init; }
}