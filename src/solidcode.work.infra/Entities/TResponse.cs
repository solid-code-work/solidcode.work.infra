using System;
namespace solidcode.work.infra.Entities;

public class TResponse
{
    public bool IsSuccess { get; init; }
    public int StatusCode { get; init; }
    public string? Message { get; init; }
}


public class TResponse<T> : TResponse
{
    public T? Data { get; init; }
}


