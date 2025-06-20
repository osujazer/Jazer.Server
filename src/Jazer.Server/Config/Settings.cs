using System.ComponentModel.DataAnnotations;

namespace Jazer.Server.Config;

public class Settings
{
    [Required]
    public required string ServiceHost { get; init; }

    [Required]
    public required int ServicePort { get; init; }

    [Required]
    public required string DatabaseConnectionString { get; init; }
    
    [Required]
    public required string JwtSecretKey { get; init; }
    
    [Required]
    public required int JwtExpiryMinutes { get; init; }
    
    [Required]
    public required string JwtIssuer { get; init; }
    
    [Required]
    public required string JwtAudience { get; init; }
    
    [Required]
    public required int RefreshTokenExpiryDays { get; init; }
}