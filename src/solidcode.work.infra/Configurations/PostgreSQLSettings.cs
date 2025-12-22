namespace solidcode.work.infra.Configurations;

public class PostgreSQLSettings
{
    public required string Host { get; init; }
    public required string Port { get; init; } = "5432";
    public required string Database { get; init; }
    public required string User { get; init; }
    public required string Pass { get; init; }

    public string ConnectionString => $"Host={Host};Port={Port};Database={Database};Username={User};Password={Pass}";
    public string ConnectionStringWithSSL => $"Host={Host};Port={Port};Database={Database};Username={User};Password={Pass};SSL Mode=Require;Trust Server Certificate=true";

}
