namespace solidcode.work.infra.Validation;

public static class ValidationMessages
{
    // General
    public const string Required = "{PropertyName} is required.";
    public const string NotEmpty = "{PropertyName} must not be empty.";
    public const string NotNull = "{PropertyName} cannot be null.";
    public const string Invalid = "{PropertyName} is invalid.";

    // String rules
    public const string MinLength = "{PropertyName} must be at least {MinLength} characters long.";
    public const string MaxLength = "{PropertyName} must not exceed {MaxLength} characters.";
    public const string LengthRange = "{PropertyName} must be between {MinLength} and {MaxLength} characters.";

    // Numeric rules
    public const string GreaterThanZero = "{PropertyName} must be greater than zero.";
    public const string GreaterOrEqualZero = "{PropertyName} must be greater than or equal to zero.";

    // Date rules
    public const string FutureDate = "{PropertyName} must be a future date.";
    public const string PastDate = "{PropertyName} must be a past date.";
    public const string NotDefaultDate = "{PropertyName} must be a valid date.";

    // Business rules
    public const string AlreadyExists = "{PropertyName} already exists.";
    public const string NotFound = "{PropertyName} was not found.";
    public const string CannotBeModified = "{PropertyName} cannot be modified.";
    public const string InvalidState = "{PropertyName} is in an invalid state for this operation.";

    // Identity / Codes
    public const string InvalidGuid = "{PropertyName} must be a valid GUID.";
    public const string InvalidCode = "{PropertyName} contains an invalid code format.";

    // Collections
    public const string CollectionEmpty = "{PropertyName} must contain at least one item.";
    public const string CollectionTooLarge = "{PropertyName} exceeds maximum allowed items.";

    // File / Media (if needed later)
    public const string FileRequired = "A file is required.";
    public const string InvalidFileType = "{PropertyName} has an invalid file type.";
    public const string FileTooLarge = "{PropertyName} exceeds maximum file size.";

    // Auth / Security (optional shared layer usage)
    public const string Unauthorized = "You are not authorized to perform this action.";
    public const string Forbidden = "You do not have permission to access this resource.";
}