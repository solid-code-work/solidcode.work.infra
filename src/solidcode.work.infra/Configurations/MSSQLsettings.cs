namespace solidcode.work.infra.Configurations;

public class MSSQLsettings
{
    public required string Host { get; init; }
    public required string Database { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }

    public bool TrustServerCertificate { get; init; } = true;

    public string ConnectionString =>
        $"Server={Host};" +
        $"Database={Database};" +
        $"User Id={Username};" +
        $"Password={Password};" +
        $"TrustServerCertificate={TrustServerCertificate};";
}