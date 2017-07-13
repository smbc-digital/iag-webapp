using System.Threading.Tasks;
using StockportWebapp.QuestionBuilder.Entities;
using Microsoft.AspNetCore.Mvc;

namespace StockportWebapp.QuestionBuilder
{
    interface IQuestionRenderer
    {
        Task<IViewComponentResult> InvokeAsync(Question question);
    }
}
