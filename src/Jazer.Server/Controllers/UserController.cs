using Asp.Versioning;
using Jazer.Server.Attributes;
using Jazer.Server.Errors;
using Jazer.Server.Extensions;
using Jazer.Server.Models;
using Jazer.Server.Services;
using Microsoft.AspNetCore.Authorization;
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
        var country = Request.Headers.TryGetValue("CF-IPCountry", out var value) ? value.ToString() : "XX";

        var result = await userService.RegisterUser(request, country, cancellationToken);

        if (result.HasError<AlreadyExistsError>())
            return TypedResults.Conflict();

        return result switch
        {
            { IsFailed: true } => TypedResults.BadRequest(new ErrorResponse(result.Errors)),
            { IsSuccess: true } => TypedResults.Created(uri: (string?)null, result.Value),
            _ => throw new InvalidOperationException()
        };
    }

    [HttpPost("login")]
    public async Task<Results<Ok<LoginUserResponse>, UnauthorizedHttpResult, BadRequest<ErrorResponse>>> LoginUser(
        [FromBody] LoginUserRequest request,
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        var result = await userService.Login(request, cancellationToken);

        if (result.HasError<AuthenticationError>())
            return TypedResults.Unauthorized();
        
        return result switch
        {
            { IsFailed: true } => TypedResults.BadRequest(new ErrorResponse(result.Errors)),
            { IsSuccess: true } => TypedResults.Ok(result.Value),
            _ => throw new InvalidOperationException()
        };
    }
    
    [HttpPost("login-refresh")]
    public async Task<Results<Ok<LoginUserResponse>, UnauthorizedHttpResult, BadRequest<ErrorResponse>>> LoginUserWithRefreshToken(
        [FromBody] LoginUserWithRefreshTokenRequest request,
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        var result = await userService.LoginWithRefreshToken(request, cancellationToken);

        if (result.HasError<AuthenticationError>())
            return TypedResults.Unauthorized();
        
        return result switch
        {
            { IsFailed: true } => TypedResults.BadRequest(new ErrorResponse(result.Errors)),
            { IsSuccess: true } => TypedResults.Ok(result.Value),
            _ => throw new InvalidOperationException()
        };
    }

    [HttpGet("{userId:int}")]
    [Authorize]
    public async Task<Results<Ok<User>, NotFound, BadRequest, BadRequest<ErrorResponse>>> GetUser(
        [FromRoute] int userId,
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        var result = await userService.FindById(userId, cancellationToken);
        
        if (result.HasError<NotFoundError>())
            return TypedResults.NotFound();
        
        return result switch
        {
            { IsFailed: true } => TypedResults.BadRequest(new ErrorResponse(result.Errors)),
            { IsSuccess: true } => TypedResults.Ok(result.Value),
            _ => throw new InvalidOperationException()
        };
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<Ok<OwnUser>> GetMe(
        [FromServices] IUserService userService)
    {
        var ownUser = await userService.GetOwnUser(User.GetUserId());
        
        return TypedResults.Ok(ownUser);
    }
}