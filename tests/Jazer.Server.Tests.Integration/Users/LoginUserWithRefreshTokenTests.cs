using FluentAssertions;
using Jazer.Server.Errors;
using Jazer.Server.Models;
using Jazer.Server.Repositories;
using Jazer.Server.Services;
using Jazer.Server.Tests.Integration.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Renci.SshNet;

namespace Jazer.Server.Tests.Integration.Users;

public class LoginUserWithRefreshTokenTests : BaseIntegrationTest
{
    private readonly IUserService _userService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LoginUserWithRefreshTokenTests(TestWebApplicationFactory factory) : base(factory)
    {
        _userService = Scope.ServiceProvider.GetRequiredService<IUserService>();
        _refreshTokenRepository = Scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();
    }
    
    [Fact]
    public async Task LoginUserWithRefreshToken_InvalidRequest_ReturnsValidationError()
    {
        // Arrange
        var request = new LoginUserWithRefreshTokenRequest
        {
            RefreshToken = string.Empty,
        };

        // Act
        var result = await _userService.LoginWithRefreshToken(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<ValidationError>().Should().BeTrue();
    }
    
    [Fact]
    public async Task LoginUserWithRefreshToken_RefreshTokenDoesNotExist_ReturnsNotFoundError()
    {
        // Arrange
        var request = new LoginUserWithRefreshTokenRequest
        {
            RefreshToken = "does-not-exist",
        };

        // Act
        var result = await _userService.LoginWithRefreshToken(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<NotFoundError>().Should().BeTrue();
    }

    [Fact]
    public async Task LoginUserWithRefreshToken_RefreshTokenExpired_ReturnsNotFoundError()
    {
        // Arrange
        const string username = "expired-token";
        const string refreshToken = "refresh-token";

        var userResult = await _userService.RegisterUser(
            new RegisterUserRequest
            {
                Username = username,
                Email = "expired-token@gmail.com",
                Password = "some-password",
            });

        await _refreshTokenRepository.Add(
            refreshToken,
            userResult.Value,
            DateTimeOffset.UtcNow.AddDays(-1));

        var request = new LoginUserWithRefreshTokenRequest
        {
            RefreshToken = refreshToken,
        };
        
        // Act
        var result = await _userService.LoginWithRefreshToken(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<NotFoundError>().Should().BeTrue();
    }

    [Fact]
    public async Task LoginUserWithRefreshToken_ValidRefreshToken_Succeeds()
    {
        // Arrange
        const string username = "valid-token";
        const string refreshToken = "refresh-token";

        var userResult = await _userService.RegisterUser(
            new RegisterUserRequest
            {
                Username = username,
                Email = "valid-token@gmail.com",
                Password = "some-password",
            });

        await _refreshTokenRepository.Add(
            refreshToken,
            userResult.Value,
            DateTimeOffset.UtcNow.AddDays(1));

        var request = new LoginUserWithRefreshTokenRequest
        {
            RefreshToken = refreshToken,
        };
        
        // Act
        var result = await _userService.LoginWithRefreshToken(request);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public async Task LoginUserWithRefreshToken_ValidRefreshToken_UpdatesRefreshTokenInDatabase()
    {
        // Arrange
        const string username = "updated-in-db";
        const string refreshToken = "refresh-token";

        var userResult = await _userService.RegisterUser(
            new RegisterUserRequest
            {
                Username = username,
                Email = "updated-in-db@gmail.com",
                Password = "some-password",
            });

        await _refreshTokenRepository.Add(
            refreshToken,
            userResult.Value,
            DateTimeOffset.UtcNow.AddDays(1));
        
        var oldRefreshToken = await DbContext.RefreshTokens
            .AsNoTracking()
            .SingleAsync(x => x.Token == refreshToken);

        var request = new LoginUserWithRefreshTokenRequest
        {
            RefreshToken = refreshToken,
        };
        
        // Act
        var result = await _userService.LoginWithRefreshToken(request);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var newRefreshToken = await DbContext.RefreshTokens
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == oldRefreshToken.Id);

        newRefreshToken.Should().NotBeNull();
        newRefreshToken.Token.Should().Be(result.Value.RefreshToken);
    }
}