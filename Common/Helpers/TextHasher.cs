using System.Security.Cryptography;

namespace iworfShop_backend_light.Common.Helpers;

public class TextHasher
{
    public static string HashIt(string text)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        
        var pbkdf2 = new Rfc2898DeriveBytes(text, salt, 100_000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);
        
        byte[] hashBytes = new byte[48];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 32);

        return Convert.ToBase64String(hashBytes);
    }

    public static bool Verify(string text, string hashedText)
    {
        byte[] hashBytes = Convert.FromBase64String(hashedText);
        
        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        var pbkdf2 = new Rfc2898DeriveBytes(text, salt, 100_000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);

        for (int i = 0; i < 32; i++)
        {
            if (hashBytes[i + 16] != hash[i])
                return false;
        }

        return true;
    }
}