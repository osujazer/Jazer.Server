using System.Text.Json.Serialization;

namespace Jazer.Server.Models;

public class LoginUserResponse
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; set; }
    
    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; set; }
}