using System.Threading.Tasks;
using Quartz;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.Scheduler
{
    public class RedirectJob : IJob
    {
        private readonly ShortUrlRedirects _shortShortUrlRedirectses;
        private readonly LegacyUrlRedirects _legacyUrlRedirects;
        private readonly IRepository _repository;

        public RedirectJob(ShortUrlRedirects shortShortUrlRedirectses, LegacyUrlRedirects legacyUrlRedirects, IRepository repository)
        {
            _shortShortUrlRedirectses = shortShortUrlRedirectses;
            _legacyUrlRedirects = legacyUrlRedirects;
            _repository = repository;
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
