using FluentResults;
using Jazer.Server.Models;

namespace Jazer.Server.Services;

public interface IUserService
{
    Task<Result<int>> RegisterUser(RegisterUserRequest request, CancellationToken cancellationToken = default);
}