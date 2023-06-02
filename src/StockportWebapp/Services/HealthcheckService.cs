namespace StockportWebapp.Services;

public interface IHealthcheckService
{
    Task<Healthcheck> Get();
}

public class HealthcheckService : IHealthcheckService
{
    private readonly string _appVersion;
    private readonly string _sha;
    private readonly IFileWrapper _fileWrapper;
    private readonly IHttpClient _httpMaker;
    private readonly IStubToUrlConverter _urlGenerator;
    private readonly string _environment;
    private readonly IApplicationConfiguration _config;
    private readonly string authenticationKey;
    private readonly string webAppClientId;
    public readonly BusinessId _businessId;

    public HealthcheckService(
        string appVersionPath, 
        string shaPath, 
        IFileWrapper fileWrapper,
        IHttpClient httpMaker, 
        IStubToUrlConverter urlGenerator, 
        string environment, 
        IApplicationConfiguration config, 
        BusinessId businessId)
    {
        _fileWrapper = fileWrapper;
        _httpMaker = httpMaker;
        _urlGenerator = urlGenerator;
        _config = config;
        _appVersion = GetFirstFileLineOrDefault(appVersionPath, "dev");
        _sha = GetFirstFileLineOrDefault(shaPath, string.Empty);
        _environment = environment;
        authenticationKey = _config.GetContentApiAuthenticationKey();
        webAppClientId = _config.GetWebAppClientId();
        _businessId = businessId;
    }

    private string GetFirstFileLineOrDefault(string filePath, string defaultValue)
    {
        if (_fileWrapper.Exists(filePath))
        {
            var firstLine = _fileWrapper.ReadAllLines(filePath).FirstOrDefault();
            if (!string.IsNullOrEmpty(firstLine))
                return firstLine.Trim();
        }
        return defaultValue.Trim();
    }

    public async Task<Healthcheck> Get()
    {
        Healthcheck healthcheck;
        var httpResponse = await _httpMaker.Get(_urlGenerator.HealthcheckUrl(), new Dictionary<string, string> {
            { "Authorization",  authenticationKey},
            { "X-ClientId",  webAppClientId}
            });

        if (httpResponse != null)
        {
            healthcheck = BuildDependencyHealthcheck(httpResponse);
        }
        else
        {
            healthcheck = new UnavailableHealthcheck();
        }

        return new Healthcheck(
            _appVersion, 
            _businessId.ToString(),
            _sha,
            new Dictionary<string, Healthcheck>() { { "contentApi", healthcheck } },
            _environment,
            new List<RedisValueData>());
    }

    private static Healthcheck BuildDependencyHealthcheck(HttpResponse httpResponse)
    {
        if (httpResponse.StatusCode != (int)HttpStatusCode.OK) return new UnavailableHealthcheck();
        var responseString = httpResponse.Content.ToString();
        return JsonConvert.DeserializeObject<Healthcheck>(responseString);
    }
}