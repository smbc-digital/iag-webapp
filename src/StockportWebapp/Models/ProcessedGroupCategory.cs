using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class ProcessedGroupCategory : IProcessedContentType
    {

        public readonly string Name;
        public readonly string Slug;
        public readonly string Icon;
        public readonly string Image;

        public ProcessedGroupCategory()
        { }

        public ProcessedGroupCategory(string name, string slug, string icon, string image)
        {
            Name = name;
            Slug = slug;
            Icon = icon;
            Image = image;
        }
    }
}
