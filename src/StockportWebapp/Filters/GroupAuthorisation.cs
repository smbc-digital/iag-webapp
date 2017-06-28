using System;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
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
        private readonly IJwtDecoder _decoder;
        private readonly CurrentEnvironment _environment;
        private readonly ILogger<GroupAuthorisation> _logger;

        public GroupAuthorisation(IApplicationConfiguration configuration, IJwtDecoder decoder, CurrentEnvironment environment, ILogger<GroupAuthorisation> logger)
        {
            _configuration = configuration;
            _decoder = decoder;
            _environment = environment;
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Get email from cookie
            var person = new LoggedInPerson();
            try
            {
                var token = context.HttpContext.Request.Cookies[CookieName()];
                if (!string.IsNullOrEmpty(token))
                {
                    person = _decoder.Decode(token);
                }
                else
                {
                    context.Result = new RedirectResult(_configuration.GetMyAccountUrl() + "?returnUrl=" + context.HttpContext.Request.GetUri(), false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Exception thrown in GroupAuthorisation, {ex.Message}");
            }

            context.ActionArguments["loggedInPerson"] = person;
        }

        private string CookieName()
        {
            switch (_environment.Name.ToUpper())
            {
                case "INT":
                    return "int_jwtCookie";
                case "QA":
                    return "qa_jwtCookie";
                case "STAGE":
                    return "staging_jwtCookie";
                default:
                    return "jwtCookie";
            }
        }
    }
}
