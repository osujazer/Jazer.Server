using FluentAssertions;
using Jazer.Server.Errors;
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
    public async Task RegisterUser_UsernameExists_ReturnsAlreadyExistsError()
    {
        // Arrange
        const string username = "username-exists";

        await _userService.RegisterUser(
            username,
            "username-exists@gmail.com",
            "some-password");
        
        // Act
        var result = await _userService.RegisterUser(
            username,
            "username-exists-1@gmail.com",
            "some-password");
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<AlreadyExistsError>().Should().BeTrue();
    }
    
    [Fact]
    public async Task RegisterUser_EmailExists_ReturnsAlreadyExistsError()
    {
        // Arrange
        const string email = "email-exists@gmail.com";

        await _userService.RegisterUser(
            "email-exists",
            email,
            "some-password");
        
        // Act
        var result = await _userService.RegisterUser(
            "email-exists-1",
            email,
            "some-password");
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<AlreadyExistsError>().Should().BeTrue();
    }
    
    [Fact]
    public async Task RegisterUser_ValidRequest_CreatesUserInDatabase()
    {
        // Act
        var result = await _userService.RegisterUser(
            "user-in-db",
            "user-in-db@gmail.com",
            "some-password");
        
        // Assert
        result.IsSuccess.Should().BeTrue();

        var user = await DbContext.Users.SingleOrDefaultAsync(x => x.Id == result.Value);

        user.Should().NotBeNull();
    }
}