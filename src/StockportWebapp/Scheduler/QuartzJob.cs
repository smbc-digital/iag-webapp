using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.Scheduler
{
    public class QuartzJob : IJob
    {
        private readonly ShortUrlRedirects _shortShortUrlRedirectses;
        private readonly LegacyUrlRedirects _legacyUrlRedirects;
        private readonly IRepository _repository;
        private readonly ILogger<QuartzJob> _logger;

        public QuartzJob(ShortUrlRedirects shortShortUrlRedirectses, LegacyUrlRedirects legacyUrlRedirects, IRepository repository, ILogger<QuartzJob> logger)
        {
            _shortShortUrlRedirectses = shortShortUrlRedirectses;
            _legacyUrlRedirects = legacyUrlRedirects;
            _repository = repository;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var response = await _repository.GetRedirects();

            var redirects = response.Content as Redirects;

            _shortShortUrlRedirectses.Redirects = redirects.ShortUrlRedirects;
            _legacyUrlRedirects.Redirects = redirects.LegacyUrlRedirects;
        }
    }
}
