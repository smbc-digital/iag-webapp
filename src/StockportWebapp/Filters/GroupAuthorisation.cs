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
            // Get details from cookie
            var token = context.HttpContext.Request.Cookies["jwtCookie"];
            var decoder = new JwtDecoder(_keys.Key);
            var person = decoder.Decode(token);

            context.ActionArguments["loggedInPerson"] = person;
        }
    }
}
