using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Jazer.Server.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal principal)
        => int.Parse(principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value!);
}