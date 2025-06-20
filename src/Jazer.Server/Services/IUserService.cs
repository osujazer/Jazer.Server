using FluentResults;
using Jazer.Server.Models;

namespace Jazer.Server.Services;

public interface IUserService
{
    Task<Result<int>> RegisterUser(RegisterUserRequest request, CancellationToken cancellationToken = default);
    
    Task<Result<LoginUserResponse>> Login(LoginUserRequest request, CancellationToken cancellationToken = default);
    
    Task<Result<User>> FindById(int id, CancellationToken cancellationToken = default);
}