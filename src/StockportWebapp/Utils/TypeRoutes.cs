using System;

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
                case "events":
                    return "/events";
                case "groups":
                    slug = slug == "groups" ? string.Empty : slug;
                    return $"/groups/{slug}";
                case "payment":
                    return $"/payment/{slug}";
                case "showcase":
                    return $"/showcase/{slug}";
                case "sia":
                    return "/sia";
                case "privacy-notices":
                    return $"/privacy-notices/{slug}";
                default:
                    return $"/{slug}";
            }
        }
    }
}
