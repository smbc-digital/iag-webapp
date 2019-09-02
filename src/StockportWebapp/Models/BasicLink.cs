using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockportWebapp.Models
{
    public class BasicLink
    {
        public string Url { get; }

        public string Text { get; }

        public BasicLink(string url, string text)
        {
            Url = url;
            Text = text;
        }
    }
}
