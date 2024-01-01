namespace Evergreen.Service.src.Abstraction;

public interface IPasswordService
{
    static abstract void HashPassword(string originalPassword, out string hashedPassword, out byte[] salt);
    static abstract bool VerifyPassword(string originalPassword, string hashedPassword, byte[] salt);

}