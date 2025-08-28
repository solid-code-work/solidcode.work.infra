namespace Play.CommonUtils.Settings;

public class MongoDBsettings
{
    public string Host { get; init; }
    public string Port { get; init; }
    public string ConnectionString => $"mongodb://{Host}:{Port}";
}