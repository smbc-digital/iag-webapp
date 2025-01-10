namespace StockportWebapp.ViewComponents;

public class TriviaViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(List<Trivia> model, string heading, string additionalClasses)
    {
        TriviaViewModel viewModel = new()
        {
            TriviaList = model,
            Heading = heading,
            AdditionalClasses = additionalClasses
        };

        return await Task.Run(() => View(viewModel));
    }
}