using System;
using System.Collections.Generic;
using StockportWebapp.Models;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using StockportWebapp.ContentFactory;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;
using System.Collections;
using StockportWebappTests_Unit.Builders;
using StockportWebappTests_Unit.Helpers;

namespace StockportWebappTests_Unit.Unit.ContentFactory
{
    public class ContentFactoryTest
    {
        private readonly ContentTypeFactory _factory;

        public ContentFactoryTest()
        {
            var tagParserContainer = new Mock<ISimpleTagParserContainer>();
            var profileTagParser = new Mock<IDynamicTagParser<Profile>>();
            var documentTagParser = new Mock<IDynamicTagParser<Document>>();
            var alertsInlineTagParser = new Mock<IDynamicTagParser<Alert>>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var s3BucketParser = new Mock<IDynamicTagParser<S3BucketSearch>>();
            var privacyNoticeTagParser = new Mock<IDynamicTagParser<PrivacyNotice>>();
            tagParserContainer.Setup(o => o.ParseAll(It.IsAny<string>(), It.IsAny<string>())).Returns("");
            s3BucketParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<S3BucketSearch>>())).Returns("");

            _factory = new ContentTypeFactory(tagParserContainer.Object, profileTagParser.Object, new MarkdownWrapper(), documentTagParser.Object, alertsInlineTagParser.Object, httpContextAccessor.Object, s3BucketParser.Object, privacyNoticeTagParser.Object);
        }

        [Fact]
        public void ItUsesSectionFactoryToBuildProcessedSectionFromSection()
        {
            var section = new Section(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Profile>(), new List<Document>(), new List<Alert>());

            var processedSection = _factory.Build<Section>(section);

            processedSection.Should().BeOfType<ProcessedSection>();
        }

        [Fact]
        public void ItUsesProfileFactoryToBuildProcessedProfileFromProfile()
        {
            var profile = new Profile(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString,
                TextHelper.AnyString, TextHelper.AnyString,
                TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Crumb>(), new List<Alert>(), TextHelper.AnyString, TextHelper.AnyString);


            var processedProfile = _factory.Build<Profile>(profile);

            processedProfile.Should().BeOfType<ProcessedProfile>();
        }

        [Fact]
        public void ItUsesArticleFactoryToBuildProcessedArticleFromArticle()
        {
            var article = new Article(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString,
                new List<Section>(), TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Crumb>(), new List<Profile>(), new List<Document>(), false, new LiveChat(TextHelper.AnyString, TextHelper.AnyString), new List<Alert>(), new Advertisement(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, DateTime.MinValue, DateTime.MinValue, false, TextHelper.AnyString, TextHelper.AnyString));

            var processedArticle = _factory.Build<Article>(article);

            processedArticle.Should().BeOfType<ProcessedArticle>();
        }

        [Fact]
        public void ItUsesNewsFactoryToBuildProcessedNewsFromNews()
        {
            var news = new News(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Crumb>(), new DateTime(), new DateTime(), new List<Alert>(), new List<string>(), new List<Document>());

            var processed = _factory.Build<News>(news);

            processed.Should().BeOfType<ProcessedNews>();
        }
    }
}