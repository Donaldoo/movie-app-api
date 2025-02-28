using FluentValidation.Results;

namespace Movie.Application.Common.Extensions;

public static class ValidationResultExtensions
{
    public static bool ContainsErrorMessage(this ValidationResult result, string error)
        => result.Errors.Any(x => x.ErrorMessage.Equals(error));
}