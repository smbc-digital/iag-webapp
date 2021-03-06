﻿using System;
using FluentAssertions;
using Moq;
using Quartz;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Scheduler;
using StockportWebapp.Services;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Scheduler
{
    public class QuartzJobTests
    {
        private readonly Mock<IRepository> _mockRepository;
        private readonly Mock<IGroupsService> _mockGroupsService;
        private readonly ShortUrlRedirects _shortUrlRedirects = new ShortUrlRedirects(new BusinessIdRedirectDictionary());
        private readonly LegacyUrlRedirects _legacyUrlRedirects = new LegacyUrlRedirects(new BusinessIdRedirectDictionary());
        private readonly Mock<ITimeProvider> _mockTimeProvider = new Mock<ITimeProvider>();
        private readonly FeatureToggles _featureToggles = new FeatureToggles { GroupArchiveEmails = true };
        private readonly QuartzJob _quartzJob;
        const string businessId = "unittest";
        public QuartzJobTests()
        {
            _mockRepository = new Mock<IRepository>();
            _mockGroupsService = new Mock<IGroupsService>();
            _quartzJob = new QuartzJob(_shortUrlRedirects, _legacyUrlRedirects, _mockRepository.Object, _mockGroupsService.Object, _mockTimeProvider.Object, _featureToggles);

            
            var shortUrlRedirectDictionary = new BusinessIdRedirectDictionary { {businessId, new RedirectDictionary { {"test1", "value1"} } }};
            var legacyUrlRedirectDictionary = new BusinessIdRedirectDictionary { { businessId, new RedirectDictionary { { "test2", "value2" } } } };

            var redirects = new Redirects(shortUrlRedirectDictionary, legacyUrlRedirectDictionary);

            _mockRepository.Setup(o => o.GetRedirects())
                .ReturnsAsync(new HttpResponse(200, redirects, string.Empty));

        }
        [Fact]
        public void ShouldUpdateTheRedirects()
        {
            _quartzJob.Execute(new Mock<IJobExecutionContext>().Object).Wait();

            _shortUrlRedirects.Redirects.Count.Should().Be(1);
            _shortUrlRedirects.Redirects.Should().ContainKey(businessId);
            _shortUrlRedirects.Redirects[businessId].Should().ContainKey("test1");

            _legacyUrlRedirects.Redirects.Count.Should().Be(1);
            _legacyUrlRedirects.Redirects.Should().ContainKey(businessId);
            _legacyUrlRedirects.Redirects[businessId].Should().ContainKey("test2");
        }
    }
}
