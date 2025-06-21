using System.Text.Json.Serialization;

namespace Jazer.Server.Models;

public class LoginUserWithRefreshTokenRequest
{
    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; init; }
}