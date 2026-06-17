namespace Solidcode.Work.Infra.Validation.Abstractions;

public sealed record ValidationError(
    string PropertyName,
    string ErrorMessage
);