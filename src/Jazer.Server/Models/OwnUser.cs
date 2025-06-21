using System.Text.Json.Serialization;

namespace Jazer.Server.Models;

public class OwnUser : User
{
    [JsonPropertyName("email")]
    public required string Email { get; init; }
}