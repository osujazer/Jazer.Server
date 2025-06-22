using FluentResults;
using FluentValidation;
using Jazer.Server.Authentication;
using Jazer.Server.Config;
using Jazer.Server.Cryptography;
using Jazer.Server.Errors;
using Jazer.Server.Models;
using Jazer.Server.Repositories;
using Mapster;
using Microsoft.Extensions.Options;

namespace Jazer.Server.Services;

public sealed class UserService(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IUserStatisticsRepository userStatisticsRepository,
    IPasswordHasher passwordHasher,
    TokenProvider tokenProvider,
    IValidator<RegisterUserRequest> registerUserRequestValidator,
    IValidator<LoginUserRequest> loginUserRequestValidator,
    IValidator<LoginUserWithRefreshTokenRequest> loginUserWithRefreshTokenRequestValidator,
    IOptions<Settings> options) : IUserService
{
    private readonly Settings _settings = options.Value;

    public async Task<Result<int>> RegisterUser(RegisterUserRequest request, string country, CancellationToken cancellationToken = default)
    {
        var validationResult = await registerUserRequestValidator.ValidateAsync(request, cancellationToken);

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
            country,
            cancellationToken);
        
        await userStatisticsRepository.Add(userId, cancellationToken);

        return userId;
    }

    public async Task<Result<LoginUserResponse>> Login(LoginUserRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await loginUserRequestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return new ValidationError(validationResult.Errors);

        var user = await userRepository.FindByUsername(request.Username, cancellationToken)
                   ?? await userRepository.FindByEmail(request.Username, cancellationToken);

        if (user is null)
            return AuthenticationError.InvalidCredentials;

        if (!passwordHasher.Verify(request.Password, user.HashedPassword))
            return AuthenticationError.InvalidCredentials;

        var token = tokenProvider.Create(user);
        var refreshToken = tokenProvider.GenerateRefreshToken();

        await refreshTokenRepository.Add(
            refreshToken,
            user.Id,
            DateTimeOffset.UtcNow.AddDays(_settings.RefreshTokenExpiryDays),
            cancellationToken);

        return new LoginUserResponse
        {
            AccessToken = token.AccessToken,
            RefreshToken = refreshToken,
            ExpiresAt = token.ExpiresAt,
        };
    }

    public async Task<Result<LoginUserResponse>> LoginWithRefreshToken(LoginUserWithRefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await loginUserWithRefreshTokenRequestValidator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
            return new ValidationError(validationResult.Errors);

        var refreshToken = await refreshTokenRepository.FindByToken(request.RefreshToken, cancellationToken);
        
        if (refreshToken is null || refreshToken.ExpiresAt < DateTimeOffset.UtcNow)
            return AuthenticationError.InvalidCredentials;

        var accessToken = tokenProvider.Create(refreshToken.User);
        var newRefreshToken = tokenProvider.GenerateRefreshToken();
        
        await refreshTokenRepository.UpdateRefreshToken(
            refreshToken.Id,
            newRefreshToken,
            DateTimeOffset.UtcNow.AddDays(_settings.RefreshTokenExpiryDays),
            cancellationToken);

        return new LoginUserResponse
        {
            AccessToken = accessToken.AccessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = accessToken.ExpiresAt,
        };
    }

    public async Task<Result<User>> FindById(int id, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.FindById(id, cancellationToken);

        if (user is null)
            return NotFoundError.UserNotFound;

        return user.Adapt<User>();
    }

    public async Task<OwnUser> GetOwnUser(int id, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.FindById(id, cancellationToken);

        return user.Adapt<OwnUser>();
    }
}