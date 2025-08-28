namespace Play.CommonUtils.Configurations;

public class MSSQLsettings
{
    public string Host { get; init; }
    public string Port { get; set; }
    public string Username { get; set; }
    public string Pass { get; set; }

    public string ConnectionString => $"{Host}{Port}{Username}{Pass}";
}