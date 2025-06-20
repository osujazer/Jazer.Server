using FluentResults;
using FluentValidation;
using Jazer.Server.Cryptography;
using Jazer.Server.Errors;
using Jazer.Server.Models;
using Jazer.Server.Repositories;

namespace Jazer.Server.Services;

public sealed class UserService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IValidator<RegisterUserRequest> validator) : IUserService
{
    public async Task<Result<int>> RegisterUser(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return new ValidationError(validationResult.Errors);

        if (await userRepository.UsernameExists(request.Username, cancellationToken) ||
            await userRepository.EmailExists(request.Email, cancellationToken))
            return AlreadyExistsError.UserAlreadyExists;

        var hashedPassword = passwordHasher.Hash(request.Password);

        var userId = await userRepository.Add(
            request.Username,
            request.Email,
            hashedPassword,
            cancellationToken);

        return userId;
    }
}