using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class GenericFeaturedItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public List<GenericFeaturedItem> SubItems { get; set; }
    }
}
