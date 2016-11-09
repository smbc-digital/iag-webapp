using FluentAssertions;
using Moq;
using Quartz;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Scheduler;
using Xunit;

namespace StockportWebappTests.Unit.Scheduler
{
    public class RedirectJobTests
    {
        [Fact]
        public void ShouldUpdateTheRedirects()
        {
            var mockRepository = new Mock<IRepository>();
            const string businessId = "unittest";
            var businessIdRedirectDictionary = new BusinessIdRedirectDictionary { {businessId, new RedirectDictionary { {"test", "value"} } }};
            mockRepository.Setup(o => o.GetRedirects())
                .ReturnsAsync(new HttpResponse(200, businessIdRedirectDictionary, string.Empty));
            var urlRedirects = new UrlRedirect(new BusinessIdRedirectDictionary());

            var redirectJob = new RedirectJob(urlRedirects, mockRepository.Object);

            redirectJob.Execute(new Mock<IJobExecutionContext>().Object).Wait();

            urlRedirects.Redirects.Count.Should().Be(1);
            urlRedirects.Redirects.Should().ContainKey(businessId);
            urlRedirects.Redirects[businessId].Should().ContainKey("test");
        }
    }
}
