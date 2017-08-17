using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Api
{
    public class FavouritesController
    {
        private FavouritesHelper favouritesHelper;
        public FavouritesController(FavouritesHelper _favouritesHelper)
        {
            favouritesHelper = _favouritesHelper;
        }

        [Route("/favourites/add")]
        public IActionResult AddGroupsFavourite([FromQuery]string slug, [FromQuery] string type)
        {
            switch (type)
            {
                case "group":
                    favouritesHelper.AddToFavourites<Group>(slug);
                    break;
                case "event":
                    favouritesHelper.AddToFavourites<Event>(slug);
                    break;
            }

            return new OkResult();
        }

        [Route("/favourites/remove")]
        public IActionResult RemoveGroupsFavourite([FromQuery]string slug, [FromQuery] string type)
        {
            switch (type)
            {
                case "group":
                    favouritesHelper.RemoveFromFavourites<Group>(slug);
                    break;
                case "event":
                    favouritesHelper.RemoveFromFavourites<Event>(slug);
                    break;
            }

            return new OkResult();
        }

        

    }
}
