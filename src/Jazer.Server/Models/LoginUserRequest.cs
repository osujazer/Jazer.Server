using System.Text.Json.Serialization;

namespace Jazer.Server.Models;

public class LoginUserRequest
{
    [JsonPropertyName("username")]
    public required string Username { get; init; }
    
    [JsonPropertyName("password")]
    public required string Password { get; init; }
}