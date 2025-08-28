namespace Play.CommonUtils.Configurations;

public class PostgreSQLSettings
{
    public string Host { get; init; }
    public string Port { get; init; } = "5432";
    public string Database { get; init; }
    public string User { get; init; }
    public string Pass { get; init; }

    public string ConnectionString => $"Host={Host};Port={Port};Database={Database};Username={User};Password={Pass}";
    public string ConnectionStringWithSSL => $"Host={Host};Port={Port};Database={Database};Username={User};Password={Pass};SSL Mode=Require;Trust Server Certificate=true";

}
