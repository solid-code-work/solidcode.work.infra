using MassTransit.Configuration;

namespace solidcode.work.infra.Entities;

public interface IListEditEntity
{
    public bool IsNew { get; set; }
}