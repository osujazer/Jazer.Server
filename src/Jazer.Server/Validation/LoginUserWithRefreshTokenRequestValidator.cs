using FluentValidation;
using Jazer.Server.Models;

namespace Jazer.Server.Validation;

public class LoginUserWithRefreshTokenRequestValidator : AbstractValidator<LoginUserWithRefreshTokenRequest>
{
    public LoginUserWithRefreshTokenRequestValidator()
    {
        RuleFor(request => request.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required");
    }
}