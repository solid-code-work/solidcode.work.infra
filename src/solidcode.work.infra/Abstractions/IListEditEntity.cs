using MassTransit.Configuration;

namespace Solidcode.Work.Infra.Abstractions;

public interface IListEditEntity
{
    public bool IsNew { get; set; }
}