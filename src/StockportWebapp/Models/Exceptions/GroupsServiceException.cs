namespace StockportWebapp.Models.Exceptions;

public class GroupsServiceException(string message) : Exception(message)
{
}