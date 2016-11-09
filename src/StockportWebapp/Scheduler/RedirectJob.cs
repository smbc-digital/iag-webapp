using System.Threading.Tasks;
using Quartz;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.Scheduler
{
    public class RedirectJob : IJob
    {
        private readonly UrlRedirect _urlRedirect;
        private readonly IRepository _repository;

        public RedirectJob(UrlRedirect urlRedirect, IRepository repository)
        {
            _urlRedirect = urlRedirect;
            _repository = repository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var response = await _repository.GetRedirects();

            var redirect = response.Content as BusinessIdRedirectDictionary;
            _urlRedirect.Redirects = redirect;
        }
    }
}
