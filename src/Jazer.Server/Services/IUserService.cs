using FluentResults;

namespace Jazer.Server.Services;

public interface IUserService
{
    Task<Result<int>> RegisterUser(string username, string email, string password, CancellationToken cancellationToken = default);
}