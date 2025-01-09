namespace StockportWebapp.Filters;

[ExcludeFromCodeCoverage]
public class GroupAuthorisation(IApplicationConfiguration configuration,
                                ILoggedInHelper loggedInHelper) : ActionFilterAttribute
{
    private readonly IApplicationConfiguration _configuration = configuration;
    private readonly ILoggedInHelper _loggedInHelper = loggedInHelper;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        LoggedInPerson person = _loggedInHelper.GetLoggedInPerson();

        if (string.IsNullOrEmpty(person.Email))
            context.Result = new RedirectResult(_configuration.GetMyAccountUrl() + "?returnUrl=" + context.HttpContext.Request.GetDisplayUrl(), false);

        context.ActionArguments["loggedInPerson"] = person;
    }
}