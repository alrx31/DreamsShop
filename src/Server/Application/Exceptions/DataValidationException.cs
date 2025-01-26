namespace Application.Exceptions;

public class DataValidationException : Exception
{
    private const string DefaultMessage = "The requested resource was not valid.";
    
    public DataValidationException() : base(DefaultMessage) { }
    public DataValidationException(string message) : base(message) { }
    public DataValidationException(string message, Exception innerException) : base(message, innerException) { }
}