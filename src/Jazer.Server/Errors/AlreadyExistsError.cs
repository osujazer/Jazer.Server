using FluentResults;

namespace Jazer.Server.Errors;

public class AlreadyExistsError(string entity) : Error($"The {entity} already exists")
{
    public static readonly AlreadyExistsError UserAlreadyExists = new AlreadyExistsError("user");
}