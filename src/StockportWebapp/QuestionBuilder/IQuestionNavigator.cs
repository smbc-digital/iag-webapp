using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder
{
    public interface IQuestionNavigator
    {
        Page GetPage(int pageId);
        Page GetNextPage(int currentPageId);
        Task<IActionResult> RunBehaviours(Page page);
    }
}