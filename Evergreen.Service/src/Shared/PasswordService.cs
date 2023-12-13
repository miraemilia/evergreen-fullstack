using System.Security.Cryptography;
using System.Text;

namespace Evergreen.Service.src.Shared;

public class PasswordService
{
    public static void HashPassword(string originalPassword, out string hashedPassword, out byte[] salt)
    {
        var hmac = new HMACSHA256();
        salt = hmac.Key;
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(originalPassword));
        hashedPassword = BitConverter.ToString(hash).Replace("-","");
    }

    public static bool VerifyPassword(string originalPassword, string hashedPassword, byte[] salt)
    {
        var hmac = new HMACSHA256(salt);
        return BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(originalPassword))).Replace("-","") == hashedPassword;
    }
}