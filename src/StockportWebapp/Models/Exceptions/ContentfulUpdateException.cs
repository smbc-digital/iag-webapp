namespace StockportWebapp.Models.Exceptions;

public class ContentfulUpdateException : Exception
{
    public ContentfulUpdateException(string message) : base(message) { }
}