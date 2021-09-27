using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class GenericFeaturedItemList
    {
        public List<GenericFeaturedItem> Items { get; set; }
        public string ButtonText { get; set; }
        public string ButtonCssClass { get; set; }
        public bool HideButton { get; set; }
    }
}