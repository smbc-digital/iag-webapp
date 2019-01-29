using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ViewModels
{
    public class SocialMediaLinksViewModel
    {
        public string SocialMediaLinksSubheading { get; set; }

        public IEnumerable<SocialMediaLink> SocialMediaLinks { get; set; }
    }
}
