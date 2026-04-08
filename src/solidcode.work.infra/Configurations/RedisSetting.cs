namespace solidcode.work.infra.Configurations;

public class RedisSetting
{
    public required string Host { get; init; }
    public required string Port { get; init; }
    public string ConnectionString => $"{Host}:{Port}";
}