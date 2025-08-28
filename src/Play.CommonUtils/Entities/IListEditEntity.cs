using MassTransit.Configuration;

namespace Play.CommonUtils.Entities;

public interface IListEditEntity
{
    public bool IsNew { get; set; }
}