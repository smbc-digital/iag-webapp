namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class Favourites
{
    public List<Crumb> Crumbs { get; set; }
    public string Type { get; set; }
    public string FavouritesUrl { get; set; }
}