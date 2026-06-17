using FluentValidation.Results;
using Solidcode.Work.Infra.Entities;
using Solidcode.Work.Infra.Validation.Abstractions;

namespace Solidcode.Work.Infra.Validation;

public static class ValidationExceptionMapper
{
    public static TResponse ToResponse(
        IEnumerable<ValidationFailure> failures)
    {
        var errors = failures
            .Select(x => $"{x.PropertyName}: {x.ErrorMessage}")
            .Distinct()
            .ToList();

        return TResponseFactory.BadRequest(
            string.Join(Environment.NewLine, errors));
    }

    public static TResponse<T> ToResponse<T>(
        IEnumerable<ValidationFailure> failures)
    {
        var errors = failures
            .Select(x => $"{x.PropertyName}: {x.ErrorMessage}")
            .Distinct()
            .ToList();

        return TResponseFactory.BadRequest<T>(
            string.Join(Environment.NewLine, errors));
    }

    public static List<ValidationError> ToValidationErrors(
        IEnumerable<ValidationFailure> failures)
    {
        return failures
            .Select(x => new ValidationError(
                x.PropertyName,
                x.ErrorMessage))
            .ToList();
    }
}