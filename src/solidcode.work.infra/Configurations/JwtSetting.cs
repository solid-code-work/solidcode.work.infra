using Microsoft.IdentityModel.Tokens;

namespace Solidcode.Work.Infra.Configurations;

public sealed class JwtSetting
{
    public string SecretKey { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int ExpiryMinutes { get; init; } = 60;
    public int RefreshTokenExpiryDays { get; init; } = 7;
    public string SigningAlgorithm { get; init; } = SecurityAlgorithms.HmacSha256;
}







