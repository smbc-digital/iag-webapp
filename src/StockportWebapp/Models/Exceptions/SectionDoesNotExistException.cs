namespace StockportWebapp.Models.Exceptions;

public class SectionDoesNotExistException(string message) : Exception(message)
{
    public new readonly string Message = message;
}