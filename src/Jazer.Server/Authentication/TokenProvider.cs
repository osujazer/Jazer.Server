using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Jazer.Server.Config;
using Jazer.Server.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Jazer.Server.Authentication;

public class TokenProvider(IOptions<Settings> options)
{
    private const int RefreshTokenSize = 32;

    private readonly Settings _settings = options.Value;
    
    public Token Create(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JwtSecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            ]),
            Expires = DateTime.UtcNow.AddMinutes(_settings.JwtExpiryMinutes),
            SigningCredentials = credentials,
            Issuer = _settings.JwtIssuer,
            Audience = _settings.JwtAudience,
        };

        var handler = new JsonWebTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);

        return new Token(
            token,
            new DateTimeOffset(tokenDescriptor.Expires!.Value, TimeSpan.Zero));
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(RefreshTokenSize));
    }

    public record Token(string AccessToken, DateTimeOffset ExpiresAt);
}