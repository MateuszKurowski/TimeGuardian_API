namespace TimeGuardian_API.Exceptions;

public class LoginException : Exception
{
    public LoginException(string message = "Invalid username or password.") : base(message)
    {

    }
}