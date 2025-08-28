namespace Play.CommonUtils.Entities;

public class TResult<T> where T : class, new()
{
    public T Data { get; set; }
    public List<OutputItem> OutputItems { get; set; }
    public TResult()
    {
        OutputItems = new List<OutputItem>();
        Data = new T();
    }
    public bool RequestSuccess { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Warning { get; set; }
    public string Fkey { get; set; }
    public int PkIntValue { get; set; }
    public MessageErrorType? MessageErrorType { get; set; }
}
