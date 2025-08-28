namespace Play.CommonUtils.Configurations;

public class RedisSettings
{
    public string Host { get; init; }
    public string Port { get; init; }
    public string ConnectionString => $"{Host}:{Port}";
}