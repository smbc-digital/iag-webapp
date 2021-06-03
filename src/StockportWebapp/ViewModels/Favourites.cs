using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ViewModels
{
    public class Favourites
    {
        public List<Crumb> Crumbs { get; set; }
        public string Type { get; set; }
        public string FavouritesUrl { get; set; }
    }
}
