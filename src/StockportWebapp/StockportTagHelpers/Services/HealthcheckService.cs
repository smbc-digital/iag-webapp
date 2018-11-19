using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StockportWebapp.Config;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Services
{
    public interface IHealthcheckService
    {
        Task<Healthcheck> Get();
    }

    public class HealthcheckService : IHealthcheckService
    {
        private readonly string _appVersion;
        private readonly string _sha;
        private readonly IFileWrapper _fileWrapper;
        private readonly FeatureToggles _featureToggles;
        private readonly HttpClient _httpMaker;
        private readonly IStubToUrlConverter _urlGenerator;
        private readonly string _environment;
        private readonly IApplicationConfiguration _config;
        private readonly string authenticationKey;
        private readonly string webAppClientId;
        public readonly BusinessId _businessId;

        public HealthcheckService(string appVersionPath, string shaPath, IFileWrapper fileWrapper,
            FeatureToggles featureToggles, HttpClient httpMaker, IStubToUrlConverter urlGenerator, string environment, IApplicationConfiguration config, BusinessId businessId)
        {
            _fileWrapper = fileWrapper;
            _featureToggles = featureToggles;
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
            try
            {
                _httpMaker.DefaultRequestHeaders.Remove("Authorization");
                _httpMaker.DefaultRequestHeaders.Add("Authorization",  authenticationKey);
                _httpMaker.DefaultRequestHeaders.Remove("X-ClientId");
                _httpMaker.DefaultRequestHeaders.Add("X-ClientId",  webAppClientId);
                var httpResponse = await _httpMaker.GetAsync(_urlGenerator.HealthcheckUrl());
                healthcheck = await BuildDependencyHealthcheck(httpResponse);
            }
            catch (HttpRequestException)
            {
                healthcheck = new UnavailableHealthcheck();
            }

            return new Healthcheck(_appVersion, _businessId.ToString(), _sha, _featureToggles,
                new Dictionary<string, Healthcheck>() {{"contentApi", healthcheck}}, _environment, new List<RedisValueData>());
        }

        private static async Task<Healthcheck> BuildDependencyHealthcheck(HttpResponseMessage httpResponse)
        {
            if (httpResponse.StatusCode != HttpStatusCode.OK) return new UnavailableHealthcheck();
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Healthcheck>(responseString);
        }
    }
}