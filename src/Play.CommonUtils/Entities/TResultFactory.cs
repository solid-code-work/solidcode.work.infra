namespace Play.CommonUtils.Entities;

public static class TResultFactory
{
    public static TResult<T> Success<T>(T? data, string message = "") where T : class
    {
        return new TResult<T> { Success = true, RequestSuccess = true, Data = data, Message = message };
    }

    public static TResult<T> Fail<T>(string message = "", MessageErrorType? errorType = null) where T : class
    {
        return new TResult<T> { Success = false, RequestSuccess = true, Message = message, Data = null, MessageErrorType = errorType };
    }

    public static TResult<object> Success(string message = "") => Success<object>(null, message);
    public static TResult<object> Fail(string message = "", MessageErrorType? errorType = null) => Fail<object>(message, errorType);
}