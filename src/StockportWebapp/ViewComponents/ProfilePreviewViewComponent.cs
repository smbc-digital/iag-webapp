using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Models;

namespace StockportWebapp.ViewComponents
{
    public class ProfilePreviewViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<Profile> profiles)
        {
            return View(profiles);
        }
    }
}
