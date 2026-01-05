using Microsoft.AspNetCore.Http;
using solidcode.work.infra.Abstraction;
using System.Security.Claims;

namespace solidcode.work.infra.security;

internal sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User =>
        _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated == true;

    public string? UserId =>
        GetClaim(ClaimTypes.NameIdentifier);

    public string? UserName =>
        GetClaim(ClaimTypes.Name);

    public string? Email =>
        GetClaim(ClaimTypes.Email);

    public IEnumerable<Claim> Claims =>
        User?.Claims ?? Enumerable.Empty<Claim>();

    public string? GetClaim(string claimType) =>
        User?.FindFirst(claimType)?.Value;
}
