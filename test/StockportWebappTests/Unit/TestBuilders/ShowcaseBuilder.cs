using System;
using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebappTests.Unit.TestBuilders
{
    public class ShowcaseBuilder
    {
        private string _title = "title";
        private string _slug = "showcase_slug";
        private string _teaser = "teaser";
        private string _subheading = "subheading";
        private string _eventSubheading = "event subheading";
        private string _eventCategory = "event category";
        private string _newsSubheading = "news subheading";
        private string _newsCategory = "news category";
        private string _heroImageUrl = "image-url.jpg";
        private IEnumerable<Crumb> _breadcrumbs = new List<Crumb>() {new Crumb("link", "title", "type")};
        private IEnumerable<Consultation> _consultations = new List<Consultation>() { new Consultation("title", DateTime.MinValue, "https://link.url") };
        private IEnumerable<SocialMediaLink> _socialMediaLinks = new List<SocialMediaLink>() { new SocialMediaLink("title", "slug", "url", "icon") };
        private IEnumerable<Event> _events = new List<Event>();
        private IEnumerable<SubItem> _featuredItems = new List<SubItem>()
        {
            new SubItem("slug", "title", "teaser", "icon", "type", "image.jpg", new List<SubItem>() {new SubItem("slug", "title", "teaser", "icon", "type", "image.jpg", new List<SubItem>()) })
        };

        public Showcase Build()
        {
            return new Showcase(_title, _slug, _teaser, _subheading, _eventCategory, _eventSubheading, _newsSubheading, _newsCategory, string.Empty, null, _heroImageUrl, _breadcrumbs, _featuredItems, _consultations, _socialMediaLinks, _events);
        }

        public ShowcaseBuilder Title(string title)
        {
            _title = title;
            return this;
        }

        public ShowcaseBuilder Slug(string slug)
        {
            _slug = slug;
            return this;
        }

        public ShowcaseBuilder Teaser(string teaser)
        {
            _teaser = teaser;
            return this;
        }

        public ShowcaseBuilder Subheading(string subheading)
        {
            _subheading = subheading;
            return this;
        }

        public ShowcaseBuilder EventSubheading(string subheading)
        {
            _eventSubheading = subheading;
            return this;
        }

        public ShowcaseBuilder EventCategory(string category)
        {
            _eventCategory = category;
            return this;
        }

        public ShowcaseBuilder HeroImageUrl(string heroImageUrl)
        {
            _heroImageUrl = heroImageUrl;
            return this;
        }

        public ShowcaseBuilder Breadcrumbs(IEnumerable<Crumb> breadcrumbs)
        {
            _breadcrumbs = breadcrumbs;
            return this;
        }

        public ShowcaseBuilder FeaturedItems(IEnumerable<SubItem> featuredItems)
        {
            _featuredItems = featuredItems;
            return this;
        }

        public ShowcaseBuilder Consultations(IEnumerable<Consultation> consultations)
        {
            _consultations = consultations;
            return this;
        }

        public ShowcaseBuilder SocialMediaLinks(IEnumerable<SocialMediaLink> socialMediaLinks)
        {
            _socialMediaLinks = socialMediaLinks;
            return this;
        }

        public ShowcaseBuilder Events(IEnumerable<Event> events)
        {
            _events = events;
            return this;
        }
    }
}
