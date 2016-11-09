using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
        private readonly Func<HttpClient> _httpMaker;
        private readonly IStubToUrlConverter _urlGenerator;

        public HealthcheckService(string appVersionPath, string shaPath, IFileWrapper fileWrapper,
            FeatureToggles featureToggles, Func<HttpClient> httpMaker, IStubToUrlConverter urlGenerator)
        {
            _fileWrapper = fileWrapper;
            _featureToggles = featureToggles;
            _httpMaker = httpMaker;
            _urlGenerator = urlGenerator;
            _appVersion = GetFirstFileLineOrDefault(appVersionPath, "dev");
            _sha = GetFirstFileLineOrDefault(shaPath, string.Empty);
        }

        private string GetFirstFileLineOrDefault(string filePath, string defaultValue)
        {
            if (_fileWrapper.Exists(filePath))
            {
                var firstLine = _fileWrapper.ReadAllLines(filePath).FirstOrDefault();
                if (!string.IsNullOrEmpty(firstLine))
                    return firstLine;
            }
            return defaultValue;
        }

        public async Task<Healthcheck> Get()
        {
            Healthcheck healthcheck;
            try
            {
                var httpResponse = await _httpMaker().GetAsync(_urlGenerator.HealthcheckUrl());
                healthcheck = await BuildDependencyHealthcheck(httpResponse);
            }
            catch (HttpRequestException)
            {
                healthcheck = new UnavailableHealthcheck();
            }

            return new Healthcheck(_appVersion, _sha, _featureToggles,
                new Dictionary<string, Healthcheck>() {{"contentApi", healthcheck}});
        }

        private static async Task<Healthcheck> BuildDependencyHealthcheck(HttpResponseMessage httpResponse)
        {
            if (httpResponse.StatusCode != HttpStatusCode.OK) return new UnavailableHealthcheck();
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Healthcheck>(responseString);
        }
    }
}