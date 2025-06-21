using Asp.Versioning;
using Jazer.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Jazer.Server.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
public class AvatarController : ControllerBase
{
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
}