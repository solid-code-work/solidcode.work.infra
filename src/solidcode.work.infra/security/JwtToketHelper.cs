using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using solidcode.work.infra.Configurations;

namespace solidcode.work.infra.security;

public sealed class JwtTokenHelper
{
    private readonly JwtSettings _settings;
    private readonly SymmetricSecurityKey _signingKey;

    public JwtTokenHelper(IOptions<JwtSettings> options)
    {
        _settings = options?.Value
            ?? throw new ArgumentNullException(nameof(options));

        ValidateSettings(_settings);

        // Create signing key once (cleaner & safer)
        _signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_settings.SecretKey));
    }

    /// <summary>
    /// Generates a signed JWT access token.
    /// Matches CurrentUser: UserId, UserName, Email, Role
    /// </summary>
    public string GenerateToken(
        Guid userId,
        string userName,
        string email,
        IEnumerable<string> roles,
        IDictionary<string, string>? additionalClaims = null)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("userId is required.", nameof(userId));

        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("userName is required.", nameof(userName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("email is required.", nameof(email));

        var claims = new List<Claim>
        {
            // -----------------------------
            // Required for CurrentUser
            // -----------------------------
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Email, email),

            // -----------------------------
            // JWT Standard Claims
            // -----------------------------
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(
                JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };

        // -----------------------------
        // Roles
        // -----------------------------
        if (roles != null)
        {
            foreach (var role in roles)
            {
                if (!string.IsNullOrWhiteSpace(role))
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }
        }

        // -----------------------------
        // Additional Custom Claims
        // -----------------------------
        if (additionalClaims != null)
        {
            foreach (var claim in additionalClaims)
            {
                if (!string.IsNullOrWhiteSpace(claim.Key) &&
                    !string.IsNullOrWhiteSpace(claim.Value))
                {
                    claims.Add(new Claim(claim.Key, claim.Value));
                }
            }
        }

        var credentials = new SigningCredentials(
            _signingKey,
            SecurityAlgorithms.HmacSha256);

        // ⚠️ IMPORTANT:
        // Do NOT set notBefore (nbf) – avoids clock skew issues
        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static void ValidateSettings(JwtSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.SecretKey))
            throw new InvalidOperationException("JwtSettings:SecretKey is missing.");

        if (settings.SecretKey.Length < 32)
            throw new InvalidOperationException(
                "JwtSettings:SecretKey must be at least 32 characters long.");

        if (string.IsNullOrWhiteSpace(settings.Issuer))
            throw new InvalidOperationException("JwtSettings:Issuer is missing.");

        if (string.IsNullOrWhiteSpace(settings.Audience))
            throw new InvalidOperationException("JwtSettings:Audience is missing.");

        if (settings.ExpiryMinutes <= 0)
            throw new InvalidOperationException(
                "JwtSettings:ExpiryMinutes must be greater than zero.");
    }
}
