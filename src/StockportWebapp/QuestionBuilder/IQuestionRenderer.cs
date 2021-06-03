using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder
{
    interface IQuestionRenderer
    {
        Task<IViewComponentResult> InvokeAsync(Question question);
    }
}
