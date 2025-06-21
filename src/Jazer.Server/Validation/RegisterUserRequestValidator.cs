using FluentValidation;
using Jazer.Server.Models;

namespace Jazer.Server.Validation;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(request => request.Username)
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long")
            .MaximumLength(16).WithMessage("Username cannot be longer than 16 characters");

        RuleFor(request => request.Email)
            .MaximumLength(90).WithMessage("Email cannot be longer than 90 characters")
            .EmailAddress().WithMessage("Invalid email address");

        // TODO: expand on this for password strength
        RuleFor(request => request.Password)
            .MinimumLength(8).WithMessage("Password must be at least 8 characters");
    }
}