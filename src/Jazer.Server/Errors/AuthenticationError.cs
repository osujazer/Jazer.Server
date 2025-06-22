using FluentResults;

namespace Jazer.Server.Errors;

public class AuthenticationError(string error) : Error(error)
{
    public static readonly AuthenticationError InvalidCredentials = new AuthenticationError("Invalid credentials");
}