using StockportWebapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockportWebapp.ViewModels
{
    public class Favourites
    {
        public List<Crumb> Crumbs { get; set; }
        public string Type { get; set; }
        public string FavouritesUrl { get; set; }
    }
}
