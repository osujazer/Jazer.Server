using FluentAssertions;
using Jazer.Server.Errors;
using Jazer.Server.Models;
using Jazer.Server.Services;
using Jazer.Server.Tests.Integration.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Jazer.Server.Tests.Integration.Users;

public class RegisterUserTests : BaseIntegrationTest
{
    private readonly IUserService _userService;

    public RegisterUserTests(TestWebApplicationFactory factory) : base(factory)
    {
        _userService = Scope.ServiceProvider.GetRequiredService<IUserService>();
    }

    [Fact]
    public async Task RegisterUser_NoUsername_ReturnsValidationError()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            Username = string.Empty,
            Email = "no-username@gmail.com",
            Password = "some-password",
        };
        
        // Act
        var result = await _userService.RegisterUser(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<ValidationError>().Should().BeTrue();
    }

    [Fact]
    public async Task RegisterUser_UsernameTooShort_ReturnsValidationError()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            Username = "1",
            Email = "short-username@gmail.com",
            Password = "some-password",
        };
        
        // Act
        var result = await _userService.RegisterUser(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<ValidationError>().Should().BeTrue();
    }
    
    [Fact]
    public async Task RegisterUser_UsernameTooLong_ReturnsValidationError()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            Username = "username-too-looooooooong",
            Email = "long-username@gmail.com",
            Password = "some-password",
        };
        
        // Act
        var result = await _userService.RegisterUser(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<ValidationError>().Should().BeTrue();
    }
    
    
    [Fact]
    public async Task RegisterUser_NoEmail_ReturnsValidationError()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            Username = "no-email",
            Email = string.Empty,
            Password = "some-password",
        };
        
        // Act
        var result = await _userService.RegisterUser(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<ValidationError>().Should().BeTrue();
    }
    
    [Fact]
    public async Task RegisterUser_InvalidEmail_ReturnsValidationError()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            Username = "invalid-email",
            Email = "fake.email@",
            Password = "some-password",
        };
        
        // Act
        var result = await _userService.RegisterUser(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<ValidationError>().Should().BeTrue();
    }
    
    [Fact]
    public async Task RegisterUser_EmailTooLong_ReturnsValidationError()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            Username = "long-email",
            Email = "sooooooooooooooooome-suuuuuuuuuuuuuuuuuuuper-looooooooooooooong-eeeeemaiiiiiiiiiiiil@gmail.com",
            Password = "some-password",
        };
        
        // Act
        var result = await _userService.RegisterUser(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<ValidationError>().Should().BeTrue();
    }
    
    [Fact]
    public async Task RegisterUser_NoPassword_ReturnsValidationError()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            Username = "no-password",
            Email = "no-password@gmail.com",
            Password = string.Empty,
        };
        
        // Act
        var result = await _userService.RegisterUser(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<ValidationError>().Should().BeTrue();
    }
    
    [Fact]
    public async Task RegisterUser_PasswordTooShort_ReturnsValidationError()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            Username = "bad-password",
            Email = "bad-password@gmail.com",
            Password = "123",
        };
        
        // Act
        var result = await _userService.RegisterUser(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<ValidationError>().Should().BeTrue();
    }

    [Fact]
    public async Task RegisterUser_UsernameExists_ReturnsAlreadyExistsError()
    {
        // Arrange
        const string username = "username-exists";

        var firstRequest = new RegisterUserRequest
        {
            Username = username,
            Email = "username-exists@gmail.com",
            Password = "some-password",
        };

        await _userService.RegisterUser(firstRequest);

        var newRequest = new RegisterUserRequest
        {
            Username = username,
            Email = "username-exists-1@gmail.com",
            Password = "some-password",
        };
        
        // Act
        var result = await _userService.RegisterUser(newRequest);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<AlreadyExistsError>().Should().BeTrue();
    }
    
    [Fact]
    public async Task RegisterUser_EmailExists_ReturnsAlreadyExistsError()
    {
        // Arrange
        const string email = "email-exists@gmail.com";

        var firstRequest = new RegisterUserRequest
        {
            Username = "email-exists",
            Email = email,
            Password = "some-password",
        };

        await _userService.RegisterUser(firstRequest);

        var newRequest = new RegisterUserRequest
        {
            Username = "email-exists-1",
            Email = email,
            Password = "some-password",
        };
        
        // Act
        var result = await _userService.RegisterUser(newRequest);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<AlreadyExistsError>().Should().BeTrue();
    }
    
    [Fact]
    public async Task RegisterUser_ValidRequest_CreatesUserInDatabase()
    {
        // Act
        var result = await _userService.RegisterUser(
            new RegisterUserRequest
            {
                Username = "user-in-db",
                Email = "user-in-db@gmail.com",
                Password = "some-password",
            });
        
        // Assert
        result.IsSuccess.Should().BeTrue();

        var user = await DbContext.Users.SingleOrDefaultAsync(x => x.Id == result.Value);

        user.Should().NotBeNull();
    }
}