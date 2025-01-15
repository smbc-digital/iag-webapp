namespace StockportWebapp.Models.Exceptions;

public class InvalidJwtException(string message) : Exception(message)
{
}