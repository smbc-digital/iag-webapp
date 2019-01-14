using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Enums;
using StockportWebapp.Models;

namespace StockportWebapp.ViewComponents
{
    public class InformationListViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<InformationList> model, string type)
        {
            return View(new Tuple<IEnumerable<InformationList>, string>( model, type));
        }
    }
}
