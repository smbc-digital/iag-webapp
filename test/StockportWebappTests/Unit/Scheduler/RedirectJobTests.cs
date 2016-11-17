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

            var shortUrlRedirectDictionary = new BusinessIdRedirectDictionary { {businessId, new RedirectDictionary { {"test1", "value1"} } }};
            var legacyUrlRedirectDictionary = new BusinessIdRedirectDictionary { { businessId, new RedirectDictionary { { "test2", "value2" } } } };

            var redirects = new Redirects(shortUrlRedirectDictionary, legacyUrlRedirectDictionary);

            mockRepository.Setup(o => o.GetRedirects())
                .ReturnsAsync(new HttpResponse(200, redirects, string.Empty));

            var shortUrlRedirects = new ShortUrlRedirects(new BusinessIdRedirectDictionary());
            var legacyUrlRedirects = new LegacyUrlRedirects(new BusinessIdRedirectDictionary());

            var redirectJob = new RedirectJob(shortUrlRedirects, legacyUrlRedirects,  mockRepository.Object);

            redirectJob.Execute(new Mock<IJobExecutionContext>().Object).Wait();

            shortUrlRedirects.Redirects.Count.Should().Be(1);
            shortUrlRedirects.Redirects.Should().ContainKey(businessId);
            shortUrlRedirects.Redirects[businessId].Should().ContainKey("test1");

            legacyUrlRedirects.Redirects.Count.Should().Be(1);
            legacyUrlRedirects.Redirects.Should().ContainKey(businessId);
            legacyUrlRedirects.Redirects[businessId].Should().ContainKey("test2");
        }
    }
}
