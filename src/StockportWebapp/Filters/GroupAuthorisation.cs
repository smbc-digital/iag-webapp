namespace StockportWebapp.Filters;

public class GroupAuthorisation : ActionFilterAttribute
{
    private readonly IApplicationConfiguration _configuration;
    private readonly ILoggedInHelper _loggedInHelper;

    public GroupAuthorisation(
        IApplicationConfiguration configuration,
        ILoggedInHelper loggedInHelper)
    {
        _configuration = configuration;
        _loggedInHelper = loggedInHelper;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var person = _loggedInHelper.GetLoggedInPerson();

        if (string.IsNullOrEmpty(person.Email))
            context.Result = new RedirectResult(_configuration.GetMyAccountUrl() + "?returnUrl=" + context.HttpContext.Request.GetDisplayUrl(), false);

        context.ActionArguments["loggedInPerson"] = person;
    }
}
