namespace StockportWebapp.Exceptions
{
    public class SectionDoesNotExistException : Exception
    {
        public new readonly string Message;

        public SectionDoesNotExistException(string message) : base(message)
        {
            Message = message;
        }
    }
}
