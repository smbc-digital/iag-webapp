using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.Services.Profile.Entities
{
    public class ProfileEntity
    {
        public string Title { get; }
        public string Slug { get; }
        public string LeadParagraph { get; }
        public string Teaser { get; }
        public string Image { get; }
        public string Body { get; }
        public IEnumerable<Crumb> Breadcrumbs { get; }
        public List<Alert> Alerts { get; }
    }
}