namespace solidcode.work.infra.Configurations;

public class MSSQLsettings
{
    public required string Host { get; init; }
    public required string Port { get; set; }
    public required string Username { get; set; }
    public required string Pass { get; set; }

    public string ConnectionString => $"{Host}{Port}{Username}{Pass}";
}