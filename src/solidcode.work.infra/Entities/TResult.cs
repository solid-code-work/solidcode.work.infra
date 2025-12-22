namespace solidcode.work.infra.Entities;

public class TResult<T>
{
    public bool IsSuccess { get; init; }              // Success flag
    public int StatusCode { get; init; }              // HTTP status code (200, 400, 404, etc.)
    public T? Data { get; init; }                     // Payload
    public string? Message { get; init; }             // Success or error message
    public MessageErrorType? ErrorType { get; init; } // Enum for error classification
    public List<OutputItem>? OutputItems { get; init; } // Optional extra info
    public string? Warning { get; init; }             // Optional warning
}
