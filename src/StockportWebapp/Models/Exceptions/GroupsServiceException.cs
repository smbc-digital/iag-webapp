namespace StockportWebapp.Models.Exceptions;

public class GroupsServiceException : Exception
{
    public GroupsServiceException(string message) : base(message) { }
}
