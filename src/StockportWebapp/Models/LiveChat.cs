using StockportWebapp.Utils;

namespace StockportWebapp.Models
{
    public class LiveChat
    {
        public string Title { get; set; }
        public string Text { get; set; }

        public LiveChat(string title, string text)
        {
            Title = title;
            Text = text;
        }

        public class NullLiveChat : LiveChat
        {
            public NullLiveChat() : base(string.Empty, string.Empty) { }
        }
    }
}
