using System.Security.Cryptography;

namespace Jazer.Server.Cryptography;

public sealed class Pbkdf2PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100_000;
    private const char Separator = '-';
    
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return $"{Convert.ToHexString(hash)}{Separator}{Convert.ToHexString(salt)}";
    }

    public bool Verify(string password, string passwordHash)
    {
        var parts = passwordHash.Split(Separator);

        var hash = Convert.FromHexString(parts[0]);
        var salt = Convert.FromHexString(parts[1]);
        
        var inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }
}