using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location=ResponseCacheLocation.Any,Duration= Cache.DefaultDuration)]
    public class ErrorController : Controller
    {
        public async Task<IActionResult> Error(string id)
        {
            if (id.Equals("404"))
            {
                ViewData["ErrorHeading"] = "Something's missing";
                ViewData["ErrorMessage"] = "Sorry, the page you are looking for cannot be found. "
                                           + "It may have been removed, had its name changed, or is temporarily unavailable." ;
            } else{
                ViewData["ErrorHeading"] = "Something went wrong";
                ViewData["ErrorMessage"] = "An unexpected error occurred and the page you are looking for cannot be found.";
            }

            return await Task.FromResult(View());
        }
    }
}