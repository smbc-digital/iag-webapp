using System.Collections.Generic;

namespace StockportWebapp.Utils
{
    public class NewsFilter
    {
        private readonly CurrentUrl currentUrl;

        public NewsFilter(CurrentUrl currentUrl)
        {
            this.currentUrl = currentUrl;
        }
    }
}