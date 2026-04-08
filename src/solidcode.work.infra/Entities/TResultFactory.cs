namespace solidcode.work.infra.Entities;

public static class TResponseFactory
{
    // ========================
    // SUCCESS
    // ========================

    public static TResponse Ok(string? message = null) =>
        new()
        {
            IsSuccess = true,
            StatusCode = 200,
            Message = message
        };

    public static TResponse<T> Ok<T>(T data, string? message = null) =>
        new()
        {
            IsSuccess = true,
            StatusCode = 200,
            Data = data,
            Message = message
        };

    public static TResponse Created(string? message = null) =>
        new()
        {
            IsSuccess = true,
            StatusCode = 201,
            Message = message
        };

    public static TResponse<T> Created<T>(T data, string? message = null) =>
        new()
        {
            IsSuccess = true,
            StatusCode = 201,
            Data = data,
            Message = message
        };

    public static TResponse NoContent(string? message = null) =>
        new()
        {
            IsSuccess = true,
            StatusCode = 204,
            Message = message
        };

    public static TResponse<T> NoContent<T>(string? message = null) =>
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

    public static TResponse BadRequest(string message) =>
        new()
        {
            IsSuccess = false,
            StatusCode = 400,
            Message = message
        };

    public static TResponse<T> BadRequest<T>(string message) =>
        new()
        {
            IsSuccess = false,
            StatusCode = 400,
            Message = message
        };

    public static TResponse NotFound(string message) =>
        new()
        {
            IsSuccess = false,
            StatusCode = 404,
            Message = message
        };

    public static TResponse<T> NotFound<T>(string message) =>
        new()
        {
            IsSuccess = false,
            StatusCode = 404,
            Message = message
        };

    // ========================
    // SERVER ERROR
    // ========================

    public static TResponse Error(string message) =>
        new()
        {
            IsSuccess = false,
            StatusCode = 500,
            Message = message
        };

    public static TResponse<T> Error<T>(string message) =>
        new()
        {
            IsSuccess = false,
            StatusCode = 500,
            Message = message
        };
}
