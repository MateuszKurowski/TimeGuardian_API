namespace TimeGuardian_API.Exceptions;

public class TokenExpireException : Exception
{
    public TokenExpireException()
    {

    }

    public TokenExpireException(string message) : base(message)
    {

    }
}