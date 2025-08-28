namespace Play.CommonUtils.Entities;

public class TResult<T> : BaseResponseViewModel where T : class, new()
{
    public T Data { get; set; }
    public List<OutputItem> OutputItems { get; set; }
    public TResult()
    {
        OutputItems = new List<OutputItem>();
        Data = new T();
    }
}
