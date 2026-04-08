namespace solidcode.work.infra.Abstraction;

public interface IPasswordHashService
{
    void CreateHash(
        string password,
        out byte[] passwordHash,
        out byte[] passwordSalt);

    bool Verify(
        string password,
        byte[] storedHash,
        byte[] storedSalt);
}
