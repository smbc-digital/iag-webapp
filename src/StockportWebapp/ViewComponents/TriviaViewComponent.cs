using Microsoft.AspNetCore.Mvc;
using StockportWebapp.ProcessedModels;

namespace StockportWebapp.ViewComponents
{
    public class TriviaViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<ProcessedTrivia> model, string heading, string additionalClasses)
        {
            return await Task.Run(() => View(new Tuple<IEnumerable<ProcessedTrivia>, string, string>(model, heading, additionalClasses)));
        }
    }
}
