using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using StockportWebapp.Models;

namespace StockportWebappTests.Unit.TestBuilders
{
    public class ShowcaseBuilder
    {
        private string _title = "title";
        private string _slug = "showcase_slug";
        private string _teaser = "teaser";
        private string _subheading = "subheading";
        private string _heroImageUrl = "image-url.jpg";
        private IEnumerable<Crumb> _breadcrumbs = new List<Crumb>() {new Crumb("link", "title", "type")};
        private IEnumerable<SubItem> _featuredItems = new List<SubItem>()
        {
            new SubItem("slug", "title", "teaser", "icon", "type", "image.jpg", new List<SubItem>() {new SubItem("slug", "title", "teaser", "icon", "type", "image.jpg", new List<SubItem>()) })};

        public Showcase Build()
        {
            return new Showcase(_title, _slug, _teaser, _subheading, _heroImageUrl, _breadcrumbs, _featuredItems);
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
    }

    
}
