using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class EventHomepage
    {
        public List<EventHomepageRow> Rows { get; set; }
        public List<EventCategory> Categories { get; set; }

        public string MetaDescription { get; set; }

        public EventHomepage()
        {

        }

        public GenericFeaturedItemList GenericItemList
        {
            get
            {
                var result = new GenericFeaturedItemList();
                result.Items = new List<GenericFeaturedItem>();
                foreach (var cat in Categories)
                {
                    result.Items.Add(new GenericFeaturedItem { Icon = cat.Icon, Title = cat.Name, Url = $"/events?category={cat.Slug}" });
                }

                result.ButtonText = "View more categories";

                return result;
            }
        }
    }
}
