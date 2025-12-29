namespace solidcode.work.infra.Entities;

public class BaseResponseViewModel_old
{
    public required bool RequestSuccess { get; set; }
    public required bool Success { get; set; }
    public required string Message { get; set; }
    public required string Warning { get; set; }
    public required string Fkey { get; set; }
    public int PkIntValue { get; set; }
    public MessageErrorType? MessageErrorType { get; set; }
}
