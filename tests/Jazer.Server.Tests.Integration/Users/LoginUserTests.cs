using FluentAssertions;
using Jazer.Server.Errors;
using Jazer.Server.Models;
using Jazer.Server.Services;
using Jazer.Server.Tests.Integration.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Jazer.Server.Tests.Integration.Users;

public class LoginUserTests : BaseIntegrationTest
{
    private readonly IUserService _userService;

    public LoginUserTests(TestWebApplicationFactory factory) : base(factory)
    {
        _userService = Scope.ServiceProvider.GetRequiredService<IUserService>();
    }

    [Fact]
    public async Task LoginUser_NoUsername_ReturnsValidationError()
    {
        // Arrange
        var request = new LoginUserRequest
        {
            Username = string.Empty,
            Password = "some-password",
        };
        
        // Act
        var result = await _userService.Login(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<ValidationError>().Should().BeTrue();
    }
    
    [Fact]
    public async Task LoginUser_NoPassword_ReturnsValidationError()
    {
        // Arrange
        var request = new LoginUserRequest
        {
            Username = "no-password",
            Password = string.Empty,
        };
        
        // Act
        var result = await _userService.Login(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<ValidationError>().Should().BeTrue();
    }

    [Fact]
    public async Task LoginUser_UserDoesNotExist_ReturnsAuthenticationError()
    {
        // Arrange
        var request = new LoginUserRequest
        {
            Username = "no-user",
            Password = "some-password",
        };
        
        // Act
        var result = await _userService.Login(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<AuthenticationError>().Should().BeTrue();
    }

    [Fact]
    public async Task LoginUser_IncorrectPassword_ReturnsAuthenticationError()
    {
        // Arrange
        const string username = "wrong-pass";

        var registerRequest = new RegisterUserRequest
        {
            Username = username,
            Email = "wrong-pass@gmail.com",
            Password = "some-password",
        };

        await _userService.RegisterUser(registerRequest, country: "GB");

        var request = new LoginUserRequest
        {
            Username = username,
            Password = "some-password-1",
        };
        
        // Act
        var result = await _userService.Login(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<AuthenticationError>().Should().BeTrue();
    }

    [Fact]
    public async Task LoginUser_ValidUsername_Succeeds()
    {
        // Arrange
        const string username = "valid-username";
        const string password = "some-password";

        var registerRequest = new RegisterUserRequest
        {
            Username = username,
            Email = "valid-username@gmail.com",
            Password = password,
        };

        await _userService.RegisterUser(registerRequest, country: "GB");

        var request = new LoginUserRequest
        {
            Username = username,
            Password = password,
        };
        
        // Act
        var result = await _userService.Login(request);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public async Task LoginUser_ValidEmail_Succeeds()
    {
        // Arrange
        const string email = "valid-email@gmail.com";
        const string password = "some-password";

        var registerRequest = new RegisterUserRequest
        {
            Username = "valid-email",
            Email = email,
            Password = password,
        };

        await _userService.RegisterUser(registerRequest, country: "GB");

        var request = new LoginUserRequest
        {
            Username = email,
            Password = password,
        };
        
        // Act
        var result = await _userService.Login(request);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public async Task LoginUser_ValidUser_CreatesRefreshTokenInDatabase()
    {
        // Arrange
        const string username = "refresh-token";
        const string password = "some-password";

        var registerRequest = new RegisterUserRequest
        {
            Username = username,
            Email = "refresh-token@gmail.com",
            Password = password,
        };

        var userIdResult = await _userService.RegisterUser(registerRequest, country: "GB");

        var request = new LoginUserRequest
        {
            Username = username,
            Password = password,
        };
        
        // Act
        var result = await _userService.Login(request);
        
        // Assert
        var refreshToken =
            await DbContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == result.Value.RefreshToken);
        
        refreshToken.Should().NotBeNull();
        refreshToken.UserId.Should().Be(userIdResult.Value);
    }
}