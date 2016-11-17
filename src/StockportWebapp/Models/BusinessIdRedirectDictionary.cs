using System;
using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class BusinessIdRedirectDictionary : Dictionary<string, RedirectDictionary>{ }

    public class RedirectDictionary : Dictionary<string, string>
    {
        public RedirectDictionary() : base(StringComparer.CurrentCultureIgnoreCase) { }
    }
}