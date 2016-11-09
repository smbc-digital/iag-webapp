namespace StockportWebapp.Utils
{
    public class TypeRoutes
    {
        public static string GetUrlFor(string type, string slug)
        {
            switch (type)
            {
                case "article":
                    return $"/{slug}";
                case "topic":
                    return $"/topic/{slug}";
                case "start-page":
                    return $"/start/{slug}";
                case "news":
                    return "/news";
                default:
                    return $"/{slug}";
            }
        }
    }
}
