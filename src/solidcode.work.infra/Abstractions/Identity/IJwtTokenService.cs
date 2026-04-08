public interface IJwtTokenService
{
    string GenerateToken(Guid userId, string userName, string email, IEnumerable<string> roles, string sessionId, IDictionary<string, string>? additionalClaims = null);
}