using Microsoft.IdentityModel.Tokens;

namespace solidcode.work.infra.Configurations;

public sealed class JwtSettings
{
    public string SecretKey { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int ExpiryMinutes { get; init; } = 60;
    public int RefreshTokenExpiryDays { get; init; } = 7;
    public string SigningAlgorithm { get; init; } = SecurityAlgorithms.HmacSha256;
}







