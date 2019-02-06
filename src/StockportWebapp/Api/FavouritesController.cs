using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Api
{
    public class FavouritesController
    {
        private CookiesHelper cookiesHelper;
        private IHttpContextAccessor httpContextAccessor;
        private HostHelper hostHelper;
        private ILogger<FavouritesController> logger;

        public FavouritesController(CookiesHelper _cookiesHelper, IHttpContextAccessor _httpContextAccessor, HostHelper _hostHelper, ILogger<FavouritesController> _logger)
        {
            cookiesHelper = _cookiesHelper;
            httpContextAccessor = _httpContextAccessor;
            hostHelper = _hostHelper;
            logger = _logger;
        }

        [Route("/favourites/add")]
        public IActionResult AddGroupsFavourite([FromQuery]string slug, [FromQuery] string type)
        {
            switch (type)
            {
                case "group":
                    cookiesHelper.AddToCookies<Group>(slug, "favourites");
                    break;
                case "event":
                    cookiesHelper.AddToCookies<Event>(slug, "favourites");
                    break;
            }

            return new OkResult();
        }
        
        [Route("/favourites/nojs/add")]
        public IActionResult AddGroupsFavouriteNoJs([FromQuery]string slug, [FromQuery] string type)
        {
            switch (type)
            {
                case "group":
                    cookiesHelper.AddToCookies<Group>(slug, "favourites");
                    break;
                case "event":
                    cookiesHelper.AddToCookies<Event>(slug, "favourites");
                    break;
            }

            var referer = string.Empty;

            try {
                referer = httpContextAccessor.HttpContext.Request.Headers["referer"];
                logger.LogWarning($"NoJS.Add referer header: {referer}");
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, $"Could not get 'Referer' header from Context: {ex.Message}");
            }

            return new RedirectResult(referer);
        }

        [Route("/favourites/remove")]
        public IActionResult RemoveGroupsFavourite([FromQuery]string slug, [FromQuery] string type)
        {
            switch (type)
            {
                case "group":
                    cookiesHelper.RemoveFromCookies<Group>(slug, "favourites");
                    break;
                case "event":
                    cookiesHelper.RemoveFromCookies<Event>(slug, "favourites");
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
                    cookiesHelper.RemoveFromCookies<Group>(slug, "favourites");
                    break;
                case "event":
                    cookiesHelper.RemoveFromCookies<Event>(slug, "favourites");
                    break;
            }

            var referer = string.Empty;

            try {
                referer = httpContextAccessor.HttpContext.Request.Headers["referer"];
                logger.LogWarning($"NoJS.Remove referer header: {referer}");
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, $"Could not get 'Referer' header from Context: {ex.Message}");
            }

            return new RedirectResult(referer);
        }
    }
}
