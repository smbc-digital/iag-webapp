using System;

namespace StockportWebapp.Exceptions
{
    public class InvalidJwtException : Exception
    {
        public InvalidJwtException(string message) : base(message) { }
    }
}
