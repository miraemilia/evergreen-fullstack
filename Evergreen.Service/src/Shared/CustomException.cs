namespace Evergreen.Service.src.Shared;

public class CustomException : Exception
{
    public int StatusCode { get; set; }

    public CustomException(int statusCode, string msg): base(msg)
    {
        StatusCode = statusCode;
    }

    public static CustomException NotFoundException(string msg = "Not found")
    {
        return new CustomException (404, msg);
    }

    public static CustomException WrongCredentialsException(string msg = "Wrong credentials")
    {
        return new CustomException (401, msg);
    }

    public static CustomException EmailNotAvailable(string msg = "Email not available")
    {
        return new CustomException (400, msg);
    }
}