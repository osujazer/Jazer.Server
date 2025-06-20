using FluentResults;

namespace Jazer.Server.Errors;

public class NotFoundError(string entity) : Error($"The {entity} does not exist")
{
    public static readonly NotFoundError UserNotFound = new NotFoundError("user");
}