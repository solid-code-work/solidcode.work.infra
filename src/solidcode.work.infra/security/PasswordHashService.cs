using System.Security.Cryptography;
using System.Text;
using solidcode.work.infra.Abstraction;

namespace solidcode.work.infra.security;

public sealed class PasswordHashService : IPasswordHashService
{
    public void CreateHash(
        string password,
        out byte[] passwordHash,
        out byte[] passwordSalt)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty.");

        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash =
            hmac.ComputeHash(
                Encoding.UTF8.GetBytes(password));
    }

    public bool Verify(
        string password,
        byte[] storedHash,
        byte[] storedSalt)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        using var hmac = new HMACSHA512(storedSalt);
        var computedHash =
            hmac.ComputeHash(
                Encoding.UTF8.GetBytes(password));

        return computedHash.SequenceEqual(storedHash);
    }
}
