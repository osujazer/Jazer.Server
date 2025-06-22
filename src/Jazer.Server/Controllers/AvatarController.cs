using Asp.Versioning;
using Jazer.Server.Extensions;
using Jazer.Server.Models;
using Jazer.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Jazer.Server.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/avatars")]
[ApiVersion("1.0")]
public class AvatarController : ControllerBase
{
    private const int MaxFileSizeMb = 5;

    private static readonly ErrorResponse InvalidPngError = new ErrorResponse("Image must be valid PNG file");
    private static readonly ErrorResponse FileTooLargeError = new ErrorResponse($"File must not be larger than {MaxFileSizeMb}MB");

    [HttpGet("{userId:int}")]
    public async Task<FileContentHttpResult> GetUserAvatar(
        [FromRoute] int userId,
        [FromServices] IAvatarService avatarService,
        CancellationToken cancellationToken)
    {
        var userAvatar = await avatarService.GetAvatar(userId, cancellationToken);

        return TypedResults.File(
            userAvatar,
            contentType: "image/png");
    }

    [HttpPost]
    [Authorize]
    public async Task<Results<NoContent, UnprocessableEntity<ErrorResponse>>> UpdateUserAvatar(
        IFormFile file,
        [FromServices] IAvatarService avatarService,
        CancellationToken cancellationToken)
    {
        if (!file.IsPng())
            return TypedResults.UnprocessableEntity(InvalidPngError);
        
        if (file.Length > MaxFileSizeMb * 1024 * 1024)
            return TypedResults.UnprocessableEntity(FileTooLargeError);

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream, cancellationToken);
        
        await avatarService.SetAvatar(
            User.GetUserId(), 
            memoryStream.ToArray(),
            cancellationToken);

        return TypedResults.NoContent();
    }

    [HttpDelete]
    [Authorize]
    public async Task<NoContent> DeleteUserAvatar(
        [FromServices] IAvatarService avatarService,
        CancellationToken cancellationToken)
    {
        await avatarService.DeleteAvatar(User.GetUserId(), cancellationToken);
        
        return TypedResults.NoContent();
    }
}