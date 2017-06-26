using System;
using Microsoft.AspNetCore.Mvc.Filters;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Filters
{
    public class GroupAuthorisation : ActionFilterAttribute
    {
        private readonly GroupAuthenticationKeys _keys;

        public GroupAuthorisation(GroupAuthenticationKeys keys)
        {
            _keys = keys;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Get email from cookie
            var person = new LoggedInPerson();
            try
            {
                var token = context.HttpContext.Request.Cookies["jwtCookie"];
                var decoder = new JwtDecoder(_keys.Key);
                person = decoder.Decode(token);
            }
            catch (Exception)
            {
                // could not decode
            }

            context.ActionArguments["loggedInPerson"] = person;
        }
    }
}
