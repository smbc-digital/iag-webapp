﻿namespace StockportWebapp.Parsers
{
    public interface ISimpleTagParser
    {
        string Parse(string body, string title = null);
    }
}