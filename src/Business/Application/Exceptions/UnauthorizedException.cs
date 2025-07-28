namespace Application.Exceptions;

public class UnauthorizedException : Exception
{
    private const string DefaultMessage = "Not authorized.";
    
    public UnauthorizedException() : base(DefaultMessage) { }
    public UnauthorizedException(string message) : base(message) { }
    public UnauthorizedException(string message, Exception innerException) : base(message, innerException) { }
}