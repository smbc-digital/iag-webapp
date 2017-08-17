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

        [Route("/favourites/groups/add")]
        public IActionResult AddGroupsFavourite([FromQuery]string slug, [FromQuery] string type)
        {
            favouritesHelper.AddToFavourites<Group>(slug);

            return null;
        }

        [Route("/favourites/groups/remove")]
        public IActionResult RemoveGroupsFavourite([FromQuery]string slug, [FromQuery] string type)
        {
            favouritesHelper.RemoveFromFavourites<Group>(slug);

            return null;
        }

    }
}
