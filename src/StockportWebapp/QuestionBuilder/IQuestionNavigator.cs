using StockportWebapp.QuestionBuilder.Entities;
using Microsoft.AspNetCore.Mvc;

namespace StockportWebapp.QuestionBuilder
{
    public interface IQuestionNavigator
    {
        Page GetPage(int pageId);
        Page GetNextPage(int currentPageId);
        IActionResult RunBehaviours(Page page);
    }
}