using System.Security.Cryptography;
using System.Text;

namespace Evergreen.Service.src.Shared;

public class PasswordService
{
    public static void HashPassword(string originalPassword, out string hashedPassword, out byte[] salt)
    {
        var hmac = new HMACSHA256();
        salt = hmac.Key;
        hashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(originalPassword)).ToString()!;
    }

    public static bool VerifyPassword(string originalPassword, string hashedPassword, byte[] salt)
    {
        var hmac = new HMACSHA256(salt);
        return hmac.ComputeHash(Encoding.UTF8.GetBytes(originalPassword)).ToString() == hashedPassword;
    }
}