using System;

namespace StockportWebapp.Models
{
    public class Event
    {
        public string Title { get; }
        public string Slug { get; }
        public string Teaser { get; }
        public string Image { get; }
        public string ThumbnailImage { get; }
        public string Description { get; set; }

        public Event(string title, string slug, string teaser, string image, string thumbnailImage, string description)
        {
            Title = title;
            Slug = slug;
            Teaser = teaser;
            Image = image;
            Description = description;
            ThumbnailImage = thumbnailImage;
        }
    }

    public class NullEvent : Event
    {
        public NullEvent()
            : base(
                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty)
        { }
    }
}
