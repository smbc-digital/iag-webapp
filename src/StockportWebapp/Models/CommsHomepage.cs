using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class CommsHomepage
    {
        public string Title { get; set; }

        public CallToActionBanner CallToActionBanner { get; set; }

        public string LatestNewsHeader { get; set; }

        public string TwitterFeedHeader { get; set; }

        public string InstagramFeedTitle { get; set; }

        public string InstagramLink { get; set; }

        public string FacebookFeedTitle { get; set; }

        public IEnumerable<BasicLink> UsefullLinks { get; set; }

        public Event WhatsOnInStockportEvent { get; set; }
    }
}