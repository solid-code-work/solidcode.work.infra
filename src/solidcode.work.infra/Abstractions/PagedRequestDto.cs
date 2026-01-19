namespace solidcode.work.infra.Abstraction;
public abstract class PagedRequestDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    public int Skip => (PageNumber - 1) * PageSize;
}
