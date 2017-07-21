using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using StockportWebapp.QuestionBuilder.Entities;
using Microsoft.AspNetCore.Mvc;

namespace StockportWebapp.QuestionBuilder
{
    public class QuestionRenderer : ViewComponent, IQuestionRenderer
    {
        private const string QuestionTypeCustom = "Custom";

        public async Task<IViewComponentResult> InvokeAsync(Question question)
        {
            if (question.QuestionType == QuestionTypeCustom)
            {
                var questionRendererType =
                    GetType()
                        .GetTypeInfo()
                        .Assembly.GetTypes()
                        .FirstOrDefault(
                            t => typeof(IQuestionRenderer).IsAssignableFrom(t) && t.Name == question.QuestionRenderer);

                var questionController = (IQuestionRenderer)Activator.CreateInstance(questionRendererType);

                // If you have custom functionality, this is where it will be called from.
                return await questionController.InvokeAsync(question);
            }

            // If we're not trying to render a custom component, just return the view
            return View(question.QuestionType, question);
        }
    }
}
