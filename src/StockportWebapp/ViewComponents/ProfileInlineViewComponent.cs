using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Models;

namespace StockportWebapp.ViewComponents
{
    public class ProfileInlineViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Profile profile, bool withoutBody)
        {
            return View(profile);
        }
    }
}
