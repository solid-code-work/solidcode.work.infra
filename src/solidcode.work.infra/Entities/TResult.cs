namespace solidcode.work.infra.Entities;

public class TResult
{
    public bool IsSuccess { get; init; }
    public int StatusCode { get; init; }
    public string? Message { get; init; }
}


public class TResult<T> : TResult
{
    public T? Data { get; init; }
}


