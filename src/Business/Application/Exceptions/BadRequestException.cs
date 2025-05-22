namespace Application.Exceptions;

public class BadRequestException : Exception
{
    private const string DefaultMessage = "The Request was not valid.";

    public BadRequestException() : base(DefaultMessage) { }
    public BadRequestException(string message) : base(message) { }
    public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
}