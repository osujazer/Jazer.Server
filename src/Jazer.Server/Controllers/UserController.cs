using Asp.Versioning;
using Jazer.Server.Attributes;
using Jazer.Server.Errors;
using Jazer.Server.Models;
using Jazer.Server.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Jazer.Server.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
public class UserController : ControllerBase
{
    [HttpPost]
    [AnonymousOnly]
    public async Task<Results<Created<int>, Conflict, BadRequest<ErrorResponse>>> RegisterUser(
        [FromBody] RegisterUserRequest request,
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        var result = await userService.RegisterUser(request, cancellationToken);

        if (result.HasError<AlreadyExistsError>())
            return TypedResults.Conflict();

        return result switch
        {
            { IsFailed: true } => TypedResults.BadRequest(new ErrorResponse(result.Errors)),
            { IsSuccess: true } => TypedResults.Created(uri: (string?)null, result.Value),
            _ => throw new InvalidOperationException()
        };
    }
}