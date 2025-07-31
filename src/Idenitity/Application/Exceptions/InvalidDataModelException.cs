namespace Application.Exceptions;

public class InvalidDataModelException : Exception
{
    private const string DefaultMessage = "Invalid Data Got To The Server.";
    
    public InvalidDataModelException() : base(DefaultMessage) { }
    public InvalidDataModelException(string message) : base(message) { }
    public InvalidDataModelException(string message, Exception innerException) : base(message, innerException) { }
}