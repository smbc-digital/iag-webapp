using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.ProcessedModels;

namespace StockportWebapp.ViewComponents
{
    public class InformationListViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<ProcessedInformationItem> model, string heading, string additionalClasses)
        {
            return View(new Tuple<IEnumerable<ProcessedInformationItem>, string, string>( model, heading, additionalClasses));
        }
    }
}
