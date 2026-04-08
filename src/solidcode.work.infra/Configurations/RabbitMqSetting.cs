namespace solidcode.work.infra.Configurations;

public class RabbitMqSetting
{
    public required string Host { get; init; }
    public string VirtualHost { get; init; } = "/";
    public required string Username { get; init; }
    public required string Password { get; init; }
    public int Port { get; init; } = 5672;
}

