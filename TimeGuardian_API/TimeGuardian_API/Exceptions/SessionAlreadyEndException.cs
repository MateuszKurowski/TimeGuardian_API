namespace TimeGuardian_API.Exceptions;

public class SessionAlreadyEndException : Exception
{
    public SessionAlreadyEndException() : base()
    {
        
    }

    public SessionAlreadyEndException(string message) : base(message)
    {
        
    }
}
