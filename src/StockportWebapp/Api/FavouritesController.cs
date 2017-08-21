using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Api
{
    public class FavouritesController
    {
        private FavouritesHelper favouritesHelper;
        private IHttpContextAccessor httpContextAccessor;
        private HostHelper hostHelper;

        public FavouritesController(FavouritesHelper _favouritesHelper, IHttpContextAccessor _httpContextAccessor, HostHelper _hostHelper)
        {
            favouritesHelper = _favouritesHelper;
            httpContextAccessor = _httpContextAccessor;
            hostHelper = _hostHelper;
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

        [Route("/favourites/nojs/add")]
        public IActionResult AddGroupsFavouriteNoJs([FromQuery]string slug, [FromQuery] string type, [FromQuery] string action, [FromQuery] string controller)
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

            return new RedirectToActionResult(action, controller, null);
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

        [Route("/favourites/nojs/remove")]
        public IActionResult RemoveGroupsFavouriteNoJs([FromQuery]string slug, [FromQuery] string type)
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

            return new RedirectResult(hostHelper.GetHostAndQueryString(httpContextAccessor.HttpContext.Request));
        }



    }
}
