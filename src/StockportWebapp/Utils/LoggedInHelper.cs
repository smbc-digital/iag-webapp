namespace StockportWebapp.Utils;

public interface ILoggedInHelper
{
    LoggedInPerson GetLoggedInPerson();
}

public class LoggedInHelper : ILoggedInHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly CurrentEnvironment _environment;
    private readonly IJwtDecoder _decoder;
    private readonly ILogger<LoggedInHelper> _logger;

    public LoggedInHelper(IHttpContextAccessor httpContextAccessor, CurrentEnvironment environment, IJwtDecoder decoder, ILogger<LoggedInHelper> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _environment = environment;
        _decoder = decoder;
        _logger = logger;
    }

    public LoggedInPerson GetLoggedInPerson()
    {
        var person = new LoggedInPerson();

        try
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies[CookieName()];
            if (!string.IsNullOrEmpty(token))
            {
                person = _decoder.Decode(token);
                person.rawCookie = token;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Exception thrown in GroupAuthorisation, {ex.Message}");
        }

        return person;
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
