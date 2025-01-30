namespace StockportWebapp.Utils;

public interface ILoggedInHelper
{
    LoggedInPerson GetLoggedInPerson();
}

public class LoggedInHelper(IHttpContextAccessor httpContextAccessor,
                            CurrentEnvironment environment,
                            IJwtDecoder decoder,
                            ILogger<LoggedInHelper> logger) : ILoggedInHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly CurrentEnvironment _environment = environment;
    private readonly IJwtDecoder _decoder = decoder;
    private readonly ILogger<LoggedInHelper> _logger = logger;

    public LoggedInPerson GetLoggedInPerson()
    {
        LoggedInPerson person = new();

        try
        {
            string token = _httpContextAccessor.HttpContext.Request.Cookies[CookieName()];
            if (!string.IsNullOrEmpty(token))
            {
                person = _decoder.Decode(token);
                person.RawCookie = token;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Exception thrown in GroupAuthorisation, {ex.Message}");
        }

        return person;
    }

    private string CookieName() =>
        _environment.Name.ToUpper() switch
        {
            "INT" => "int_jwtCookie",
            "QA" => "qa_jwtCookie",
            "STAGE" => "staging_jwtCookie",
            _ => "jwtCookie",
        };
}