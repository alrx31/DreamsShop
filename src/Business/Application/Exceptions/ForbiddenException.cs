namespace Application.Exceptions;

public class ForbiddenException : Exception
{
    private const string DefaultMessage = "The Request was not allowed.";
    
    public ForbiddenException() : base(DefaultMessage) { }
    public ForbiddenException(string message) : base(message) { }
    public ForbiddenException(string message, Exception innerException) : base(message, innerException) { }
    
}