using System.Security.Claims;

namespace solidcode.work.infra.Abstraction;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }

    string? UserId { get; }
    string? UserName { get; }
    string? Email { get; }

    IEnumerable<Claim> Claims { get; }

    string? GetClaim(string claimType);
}
