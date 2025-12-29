namespace solidcode.work.infra.Entities;

public static class TResultFactory
{
    // ========================
    // SUCCESS
    // ========================

    public static TResult Ok(string? message = null) =>
        new()
        {
            IsSuccess = true,
            StatusCode = 200,
            Message = message
        };

    public static TResult<T> Ok<T>(T data, string? message = null) =>
        new()
        {
            IsSuccess = true,
            StatusCode = 200,
            Data = data,
            Message = message
        };

    public static TResult Created(string? message = null) =>
        new()
        {
            IsSuccess = true,
            StatusCode = 201,
            Message = message
        };

    public static TResult<T> Created<T>(T data, string? message = null) =>
        new()
        {
            IsSuccess = true,
            StatusCode = 201,
            Data = data,
            Message = message
        };

    public static TResult NoContent(string? message = null) =>
        new()
        {
            IsSuccess = true,
            StatusCode = 204,
            Message = message
        };

    public static TResult<T> NoContent<T>(string? message = null) =>
        new()
        {
            IsSuccess = true,
            StatusCode = 204,
            Data = default,
            Message = message
        };

    // ========================
    // CLIENT ERRORS
    // ========================

    public static TResult BadRequest(string message) =>
        new()
        {
            IsSuccess = false,
            StatusCode = 400,
            Message = message
        };

    public static TResult<T> BadRequest<T>(string message) =>
        new()
        {
            IsSuccess = false,
            StatusCode = 400,
            Message = message
        };

    public static TResult NotFound(string message) =>
        new()
        {
            IsSuccess = false,
            StatusCode = 404,
            Message = message
        };

    public static TResult<T> NotFound<T>(string message) =>
        new()
        {
            IsSuccess = false,
            StatusCode = 404,
            Message = message
        };

    // ========================
    // SERVER ERROR
    // ========================

    public static TResult Error(string message) =>
        new()
        {
            IsSuccess = false,
            StatusCode = 500,
            Message = message
        };

    public static TResult<T> Error<T>(string message) =>
        new()
        {
            IsSuccess = false,
            StatusCode = 500,
            Message = message
        };
}
