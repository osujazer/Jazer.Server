using System.Text.Json.Serialization;

namespace Jazer.Server.Models;

public class RegisterUserRequest
{
    [JsonPropertyName("username")]
    public required string Username { get; init; }
    
    [JsonPropertyName("email")]
    public required string Email { get; init; }
    
    [JsonPropertyName("password")]
    public required string Password { get; init; }
}