namespace StockportWebapp.Models.Exceptions;

public class ContentfulUpdateException(string message) : Exception(message)
{
}