using System;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using StockportWebapp.Config;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Filters
{
    public class GroupAuthorisation : ActionFilterAttribute
    {
        private readonly IApplicationConfiguration _configuration;
        private readonly ILoggedInHelper _loggedInHelper;

        public GroupAuthorisation(IApplicationConfiguration configuration, ILoggedInHelper loggedInHelper)
        {
            _configuration = configuration;
            _loggedInHelper = loggedInHelper;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var person = _loggedInHelper.GetLoggedInPerson();

            if (string.IsNullOrEmpty(person.Email)) context.Result = new RedirectResult(_configuration.GetMyAccountUrl() + "?returnUrl=" + context.HttpContext.Request.GetUri(), false);

            context.ActionArguments["loggedInPerson"] = person;
        }
    }
}
