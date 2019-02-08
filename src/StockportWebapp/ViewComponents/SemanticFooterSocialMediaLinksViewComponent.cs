using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Models;

namespace StockportWebapp.ViewComponents
{
    public class SemanticFooterSocialMediaLinksViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<SocialMediaLink> model, string additionalClasses)
        {
            return View(new Tuple<IEnumerable<SocialMediaLink>, string>(model, additionalClasses));
        }
    }
}
