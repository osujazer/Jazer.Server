using FluentResults;
using Jazer.Server.Cryptography;
using Jazer.Server.Errors;
using Jazer.Server.Repositories;

namespace Jazer.Server.Services;

public sealed class UserService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher) : IUserService
{
    public async Task<Result<int>> RegisterUser(string username, string email, string password, CancellationToken cancellationToken = default)
    {
        // TODO: validation

        if (await userRepository.UsernameExists(username, cancellationToken) ||
            await userRepository.EmailExists(email, cancellationToken))
            return AlreadyExistsError.UserAlreadyExists;

        var hashedPassword = passwordHasher.Hash(password);

        var userId = await userRepository.Add(
            username,
            email,
            hashedPassword,
            cancellationToken);

        return userId;
    }
}