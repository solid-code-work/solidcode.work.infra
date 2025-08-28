using MassTransit.Configuration;

namespace Play.CommonUtils.Entities;

public interface IListEditEntities
{
    public bool IsNew { get; set; }
}