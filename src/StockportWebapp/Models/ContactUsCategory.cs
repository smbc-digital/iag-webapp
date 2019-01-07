namespace StockportWebapp.Models
{
    public class ContactUsCategory
    {
        public string Title { get; }
        public string BodyTextLeft { get; }
        public string BodyTextRight { get; }
        public string Icon { get; set; }

        public ContactUsCategory(string title, string bodyTextLeft, string bodyTextRight, string icon)
        {
            Title = title;
            BodyTextLeft = bodyTextLeft;
            BodyTextRight = bodyTextRight;
            Icon = icon;
        }
    }

    public class NullContactUsCategory : ContactUsCategory
    {
        public NullContactUsCategory() : base(string.Empty, string.Empty, string.Empty, string.Empty) { }
    }

}
