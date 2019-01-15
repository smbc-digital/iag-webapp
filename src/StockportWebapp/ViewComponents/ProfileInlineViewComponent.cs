using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Enums;
using StockportWebapp.Models;

namespace StockportWebapp.ViewComponents
{
    public class ProfileInlineViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Profile profile, bool withoutBody)
        {
            return View(new Tuple<Profile, bool>(profile, withoutBody));
        }
    }
}
