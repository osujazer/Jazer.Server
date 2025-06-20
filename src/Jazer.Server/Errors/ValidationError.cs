using FluentResults;
using FluentValidation.Results;

namespace Jazer.Server.Errors;

public class ValidationError : Error
{
    public const string ValidationErrorMessage = "Failed to validate request";

    public ValidationError(IEnumerable<ValidationFailure> validationFailures)
    {
        Message = "Failed to validate request";
        CausedBy(validationFailures.Select(x => new Error(x.ErrorMessage)).ToList());
    }
}