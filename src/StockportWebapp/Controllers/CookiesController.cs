using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Controllers
{
    [Route("cookies")]
    public class CookiesController : Controller
    {
        private readonly ICookiesHelper _cookiesHelper;

        public CookiesController(ICookiesHelper cookiesHelper)
        {
            _cookiesHelper = cookiesHelper;
        }

        //public List<string> GetCookies(string cookieType)
        //{
        //    return _cookiesHelper.GetCookies<string>(cookieType);
        //}

        [Route("add")]
        public IActionResult AddCookie(string slug, string cookieType)
        {
            switch (cookieType)
            {
                case "group":
                    _cookiesHelper.AddToCookies<Group>(slug, "favourites");
                    break;
                case "event":
                    _cookiesHelper.AddToCookies<Event>(slug, "favourites");
                    break;
                case "alert":
                    _cookiesHelper.AddToCookies<Alert>(slug, "alerts");
                    break;
            }

            return Ok();
        }

        [Route("/remove")]
        public IActionResult RemoveGroupsFavourite(string slug, string cookieType)
        {
            switch (cookieType)
            {
                case "group":
                    _cookiesHelper.RemoveFromCookies<Group>(slug, "favourites");
                    break;
                case "event":
                    _cookiesHelper.RemoveFromCookies<Event>(slug, "favourites");
                    break;
                case "alert":
                    _cookiesHelper.AddToCookies<Alert>(slug, "alerts");
                    break;
            }

            return new OkResult();
        }
    }
}
