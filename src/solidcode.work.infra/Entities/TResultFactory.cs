namespace solidcode.work.infra.Entities;

public static class TResultFactory
{
    public static TResult<T> Ok<T>(T data, string? message = null) =>
        new TResult<T>
        {
            IsSuccess = true,
            StatusCode = 200,
            Data = data,
            Message = message,
            ErrorType = MessageErrorType.Success
        };

    public static TResult<T> BadRequest<T>(string message) =>
        new TResult<T>
        {
            IsSuccess = false,
            StatusCode = 400,
            Message = message,
            ErrorType = MessageErrorType.Validation
        };

    public static TResult<T> NotFound<T>(string message) =>
        new TResult<T>
        {
            IsSuccess = false,
            StatusCode = 404,
            Message = message,
            ErrorType = MessageErrorType.NotFound
        };

    public static TResult<T> Error<T>(string message) =>
        new TResult<T>
        {
            IsSuccess = false,
            StatusCode = 500,
            Message = message,
            ErrorType = MessageErrorType.Exception
        };

    // ✅ Add this for empty results
    public static TResult<T> Empty<T>(string message = "No content") =>
        new TResult<T>
        {
            IsSuccess = true,
            StatusCode = 204, // HTTP 204 No Content
            Data = default,
            Message = message,
            ErrorType = MessageErrorType.EmptyResult
        };
}

