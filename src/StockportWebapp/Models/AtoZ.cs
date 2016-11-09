using StockportWebapp.Utils;

namespace StockportWebapp.Models
{
    public class AtoZ
    {
        public string Title { get; }
        public string Teaser { get; }
        public string NavigationLink { get; }

        public AtoZ(string title, string slug, string teaser, string type)
        {
            Title = title;
            Teaser = teaser;
            NavigationLink = TypeRoutes.GetUrlFor(type, slug);
        }

        public class NullAtoZ : AtoZ
        {
            public NullAtoZ()
                : base(
                    string.Empty, string.Empty, string.Empty, string.Empty)
            { }
        }
    }
}
