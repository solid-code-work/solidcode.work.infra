
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using solidcode.work.infra.Configurations;

namespace solidcode.work.infra.security;

public class JwtTokenHelper
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenHelper(IConfiguration config)
    {
        _jwtSettings = config.GetSection(nameof(JwtSettings)).Get<JwtSettings>()
            ?? throw new ArgumentException("JwtSettings section is missing.");
    }

    public string GenerateToken(string userName, string role)
    {
        if (string.IsNullOrWhiteSpace(_jwtSettings.SecretKey))
            throw new ArgumentException("JwtSettings:SecretKey is missing or invalid.");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
