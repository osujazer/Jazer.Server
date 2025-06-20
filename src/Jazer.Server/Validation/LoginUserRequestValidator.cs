using FluentValidation;
using Jazer.Server.Models;

namespace Jazer.Server.Validation;

public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Username is required");
        
        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}