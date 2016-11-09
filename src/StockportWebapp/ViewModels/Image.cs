namespace StockportWebapp.ViewModels
{
    public class Image
    {
        public string cssClass { get; }
        public string url { get; }

        public Image(string cssclass, string imageUrl)
        {
            cssClass = cssclass;
            url = imageUrl;
        }
    }
}