using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.Services.Profile.Entities
{
    public class ProfileEntity
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string LeadParagraph { get; set; }
        public string Teaser { get; set; }
        public string Image { get; set; }
        public string Body { get; set; }
        public IEnumerable<Crumb> Breadcrumbs { get; set; }
        public List<Alert> Alerts { get; set; }
        public List<DidYouKnow> DidYouKnowSection { get; set; }
    }
}