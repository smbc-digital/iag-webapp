using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        
        public List<string> GetCookies()
        {
            return _cookiesHelper.GetCookies<string>("favourites");
        }
    }
}
