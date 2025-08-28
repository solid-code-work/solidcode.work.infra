namespace Play.CommonUtils.Entities;

public class BaseResponseViewModel
{
    public bool RequestSuccess { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Warning { get; set; }
    public string Fkey { get; set; }
    public int PkIntValue { get; set; }
    public MessageErrorType? MessageErrorType { get; set; }
}
