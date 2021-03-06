﻿//using System;
//using System.Collections.Generic;
//using System.IO;
//using FluentAssertions;
//using StockportWebapp.ContentFactory;
//using StockportWebapp.Http;
//using StockportWebapp.Models;
//using StockportWebapp.Utils;
//using Xunit;
//using System.Linq;
//using Microsoft.AspNetCore.Http;
//using Moq;
//using StockportWebapp.Config;
//using StockportWebapp.Parsers;
//using StockportWebapp.ProcessedModels;
//using StockportWebapp.Repositories;
//using HttpResponse = StockportWebapp.Http.HttpResponse;
//using System.Threading.Tasks;
//using StockportWebappTests_Unit.Helpers;

//namespace StockportWebappTests_Unit.Unit.Repositories
//{
//    public class ProcessedContentRepositoryTest
//    {
//        private readonly IProcessedContentRepository _repository;
//        private readonly Mock<IHttpClient> _mockHttpClient;
//        private readonly Mock<IStubToUrlConverter> _mockUrlGenerator;
//        private readonly Mock<ISimpleTagParserContainer> _tagParserContainer;
//        private readonly Mock<IDynamicTagParser<Profile>> _profileTagParser;
//        private readonly Mock<IDynamicTagParser<Document>> _documentTagParser;
//        private readonly Mock<IDynamicTagParser<Alert>> _alertsInlineTagParser;
//        private readonly Mock<MarkdownWrapper> _markdownWrapper;
//        private readonly Mock<IApplicationConfiguration> appConfig;
//        private readonly Mock<IHttpContextAccessor> httpContextAccessor;
//        private readonly Mock<IDynamicTagParser<S3BucketSearch>> _s3BucketParser;
//        private readonly Mock<IDynamicTagParser<PrivacyNotice>> _privacyNoticeTagParser;

//        public ProcessedContentRepositoryTest()
//        {
//            _tagParserContainer = new Mock<ISimpleTagParserContainer>();
//            _profileTagParser = new Mock<IDynamicTagParser<Profile>>();
//            _markdownWrapper = new Mock<MarkdownWrapper>();
//            _documentTagParser = new Mock<IDynamicTagParser<Document>>();
//            _alertsInlineTagParser = new Mock<IDynamicTagParser<Alert>>();
//            _mockUrlGenerator = new Mock<IStubToUrlConverter>();
//            appConfig = new Mock<IApplicationConfiguration>();
//            httpContextAccessor = new Mock<IHttpContextAccessor>();
//            _mockHttpClient = new Mock<IHttpClient>();
//            _s3BucketParser = new Mock<IDynamicTagParser<S3BucketSearch>>();
//            _privacyNoticeTagParser = new Mock<IDynamicTagParser<PrivacyNotice>>();

//            _s3BucketParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<S3BucketSearch>>())).Returns("");

//            var contentFactory = new ContentTypeFactory(_tagParserContainer.Object, _profileTagParser.Object, _markdownWrapper.Object, _documentTagParser.Object, _alertsInlineTagParser.Object, httpContextAccessor.Object, _s3BucketParser.Object, _privacyNoticeTagParser.Object);
//            _repository = new ProcessedContentRepository(_mockUrlGenerator.Object, _mockHttpClient.Object, contentFactory, appConfig.Object);
//        }

//        /*
//         * Article
//         */
//        [Fact]
//        public async Task GetArticleForArticleSlug()
//        {
//            var articleSlug = "physical-activity";
//            const string url = "article-with-slug-url";
//            _mockUrlGenerator.Setup(o => o.UrlFor<Article>(articleSlug, It.IsAny<List<Query>>())).Returns(url);

//            var body = "Staying active and exercising is essential to reach and maintain a healthy lifestyle.";

//            _mockHttpClient.Setup(o => o.Get(url, It.IsAny<Dictionary<string, string>>()))
//                .ReturnsAsync(new HttpResponse(200, JsonFileHelper.GetStringResponseFromFile("Article.json"), string.Empty));

//            _tagParserContainer.Setup(o => o.ParseAll(It.IsAny<string>(), It.IsAny<string>())).Returns(body);
//            _profileTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Profile>>())).Returns(body);
//            _markdownWrapper.Setup(o => o.ConvertToHtml(It.IsAny<string>())).Returns(body);
//            _documentTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Document>>())).Returns(body);
//            _alertsInlineTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Alert>>())).Returns(body);
//            _s3BucketParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<S3BucketSearch>>())).Returns(body);
//            _privacyNoticeTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<PrivacyNotice>>())).Returns(body);

//            var httpResponse = await _repository.Get<Article>(articleSlug);
//            var article = httpResponse.Content as ProcessedArticle;

//            article.Title.Should().NotBeNull();
//            article.NavigationLink.Should().NotBeNull();
//            article.Body.Should().Be(body);
//            article.BackgroundImage.Should().NotBeNull();
//            article.Icon.Should().Be("fa-icon");
//            article.Breadcrumbs.Should().HaveCount(1);
//            article.Breadcrumbs.First().Title.Should().Be("Test topic");
//            article.Breadcrumbs.First().NavigationLink.Should().Contain("test-topic");

//            var section = article.Sections.First();

//            section.Title.Should().Be("Overview ");
//            section.Slug.Should().Be("physical-activity-overview");
//            section.Body.Should().Contain("Staying active and exercising is essential to reach and maintain a healthy lifestyle.");
//        }

//        [Fact]
//        public async Task GetArticleForArticleSlugWithoutBackgroundImage()
//        {
//            const string articleSlug = "physical-activity";
//            const string url = "article-with-slug-url";
//            _mockUrlGenerator.Setup(o => o.UrlFor<Article>(articleSlug, It.IsAny<List<Query>>())).Returns(url);

//            _mockHttpClient.Setup(o => o.Get(url, It.IsAny<Dictionary<string, string>>()))
//                .ReturnsAsync(new HttpResponse(200, JsonFileHelper.GetStringResponseFromFile("ArticleWithoutBackgroundImage.json"),
//                    string.Empty));

//            var httpResponse = await _repository.Get<Article>(articleSlug);
//            var article = httpResponse.Content as ProcessedArticle;

//            article.BackgroundImage.Should().BeNull();
//            article.Title.Should().Be("Title");
//            article.Sections.First().Profiles.First().Title.Should().Be("A pull Out");
//        }

//        [Fact]
//        public async Task GetsNotFoundForArticleNotFound()
//        {
//            const string nonExistentArticle = "pineapple";
//            const string articleNotFoundError = "No article found for pineapple";

//            const string url = "non-existent-article-with-slug-url";
//            _mockUrlGenerator.Setup(o => o.UrlFor<Article>(nonExistentArticle, It.IsAny<List<Query>>())).Returns(url);

//            _mockHttpClient.Setup(o => o.Get(url, It.IsAny<Dictionary<string, string>>()))
//                .ReturnsAsync(HttpResponse.Failure(404, articleNotFoundError));

//            var httpResponse = await _repository.Get<Article>(nonExistentArticle);

//            Assert.Equal(404, httpResponse.StatusCode);
//            Assert.Equal("No article found for pineapple", httpResponse.Error);
//        }

//        [Fact]
//        public async Task GetsAlertByArticleSlug()
//        {
//            const string articleSlug = "article-with-alerts";
//            const string url = "get-articlewithalerts-with-slug-url";

//            _mockUrlGenerator.Setup(o => o.UrlFor<Article>(articleSlug, It.IsAny<List<Query>>())).Returns(url);
//            _mockHttpClient.Setup(o => o.Get(url, It.IsAny<Dictionary<string, string>>()))
//                .ReturnsAsync(new HttpResponse(200, JsonFileHelper.GetStringResponseFromFile("ArticleWithAlerts.json"),
//                    string.Empty));

//            var httpResponse = await _repository.Get<Article>(articleSlug);
//            var topic = httpResponse.Content as ProcessedArticle;

//            topic.Alerts.Should().HaveCount(1);
//            topic.Alerts.FirstOrDefault().Title.Should().Be("This is an alert");
//            topic.Alerts.FirstOrDefault().SubHeading.Should().Be("It has a subheading");
//            topic.Alerts.FirstOrDefault()
//                .Body.Should()
//                .Be("<p>It also has a body text</p>\n");
//            topic.Alerts.FirstOrDefault().Severity.Should().Be(Severity.Warning);
//        }

//        /*
//         * Profile
//         */
//        [Fact]
//        public async Task ShouldGetAProfile()
//        {
//            const string profileSlug = "slug";
//            const string url = "get-getaprofile-with-slug-url";

//            _mockUrlGenerator.Setup(o => o.UrlFor<Profile>(profileSlug, It.IsAny<List<Query>>())).Returns(url);

//            _mockHttpClient.Setup(o => o.Get(url, It.IsAny<Dictionary<string, string>>()))
//                .ReturnsAsync(new HttpResponse(200, JsonFileHelper.GetStringResponseFromFile("Profile.json"), string.Empty));

//            var httpResponse = await _repository.Get<Profile>(profileSlug);
//            var profile = httpResponse.Content as ProcessedProfile;

//            profile.Title.Should().Be("Test Profile");
//            profile.Slug.Should().Be("test-profile");
//            profile.Teaser.Should().Be("teaser");
//            profile.Subtitle.Should().Be("Test sub title");
//            profile.Image.Should().Be("image");
//            profile.Type.Should().Be("Success Story");
//            profile.BackgroundImage.Should().Be("background-image");
//            profile.Icon.Should().Be("fa-icon");
//            profile.Breadcrumbs.Should().HaveCount(2);
//            profile.Breadcrumbs.First().NavigationLink.Should().Contain("test-sub-topic-1");
//            profile.Breadcrumbs.First().Title.Should().Be("Test sub topic 1");
//        }

//        /*
//         * News
//         */
//        [Fact]
//        public async Task GetsNews()
//        {
//            const string slug = "news";
//            const string url = "get-news-with-slug-url";

//            _mockUrlGenerator.Setup(o => o.UrlFor<News>(slug, It.IsAny<List<Query>>())).Returns(url);
//            var body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";

//            _mockHttpClient.Setup(o => o.Get(url, It.IsAny<Dictionary<string, string>>())).ReturnsAsync(new HttpResponse(200, JsonFileHelper.GetStringResponseFromFile("News.json"), string.Empty));

//            _tagParserContainer.Setup(o => o.ParseAll(body, It.IsAny<string>())).Returns(body);
//            _markdownWrapper.Setup(o => o.ConvertToHtml(body)).Returns(body);
//            _documentTagParser.Setup(o => o.Parse(body, It.IsAny<List<Document>>())).Returns(body);

//            var httpResponse = await _repository.Get<News>(slug);
//            var news = httpResponse.Content as ProcessedNews;

//            news.Title.Should().Be("Another news article");
//            news.Slug.Should().Be("another-news-article");
//            news.Teaser.Should().Be("This is another news article");
//            news.Image.Should().Be("image.jpg");
//            news.ThumbnailImage.Should().Be("thumbnail.jpg");
//            news.Body.Should().Be(body);
//        }

//        [Fact]
//        public async Task GetsEvent()
//        {
//            const string slug = "event";
//            const string url = "get-event-with-slug-url";

//            _mockUrlGenerator.Setup(o => o.UrlFor<Event>(slug, null)).Returns(url);

//            var body = "The event description";

//            _mockHttpClient.Setup(o => o.Get(url, It.IsAny<Dictionary<string, string>>())).ReturnsAsync(new HttpResponse(200, JsonFileHelper.GetStringResponseFromFile("Event.json"), string.Empty));

//            _tagParserContainer.Setup(o => o.ParseAll(body, It.IsAny<string>())).Returns(body);
//            _markdownWrapper.Setup(o => o.ConvertToHtml(body)).Returns(body);
//            _documentTagParser.Setup(o => o.Parse(body, It.IsAny<List<Document>>())).Returns(body);

//            var httpResponse = await _repository.Get<Event>(slug);
//            var events = httpResponse.Content as ProcessedEvents;

//            events.Title.Should().Be("This is the event");
//            events.Slug.Should().Be("event-of-the-century");
//            events.Teaser.Should().Be("Read more for the event");

//            events.Description.Should().Be(body);
//        }

//        [Fact]
//        public async Task GetsEventWithSpecificDate()
//        {
//            const string slug = "event";
//            const string url = "get-event-with-slug-url";
//            var date = new DateTime();
//            _mockUrlGenerator.Setup(o => o.UrlFor<Event>(slug, It.Is<List<Query>>(q => q.Contains(new Query("date", date.ToString("yyyy-MM-dd")))))).Returns(url);

//            var body = "The event description";

//            _mockHttpClient.Setup(o => o.Get(url, It.IsAny<Dictionary<string, string>>())).ReturnsAsync(new HttpResponse(200, JsonFileHelper.GetStringResponseFromFile("Event.json"), string.Empty));

//            _tagParserContainer.Setup(o => o.ParseAll(body, It.IsAny<string>())).Returns(body);
//            _markdownWrapper.Setup(o => o.ConvertToHtml(body)).Returns(body);
//            _documentTagParser.Setup(o => o.Parse(body, It.IsAny<List<Document>>())).Returns(body);

//            var httpResponse = await _repository.Get<Event>(slug, new List<Query> { new Query("date", date.ToString("yyyy-MM-dd")) });
//            var eventItem = httpResponse.Content as ProcessedEvents;

//            eventItem.Title.Should().Be("This is the event");
//            eventItem.Slug.Should().Be("event-of-the-century");
//            eventItem.Teaser.Should().Be("Read more for the event");
//            eventItem.Description.Should().Be(body);
//        }

//        [Fact]
//        public async Task GetsGroup()
//        {
//            const string slug = "group";
//            const string url = "get-group-with-slug-url";

//            _mockUrlGenerator.Setup(o => o.UrlFor<Group>(slug, null)).Returns(url);

//            var body = "The group description";

//            _mockHttpClient.Setup(o => o.Get(url, It.IsAny<Dictionary<string, string>>())).ReturnsAsync(new HttpResponse(200, JsonFileHelper.GetStringResponseFromFile("Group.json"), string.Empty));

//            _tagParserContainer.Setup(o => o.ParseAll(body, It.IsAny<string>())).Returns(body);
//            _markdownWrapper.Setup(o => o.ConvertToHtml(body)).Returns(body);

//            var httpResponse = await _repository.Get<Group>(slug);
//            var group = httpResponse.Content as ProcessedGroup;

//            group.Name.Should().Be("Zumba");
//            group.Slug.Should().Be("test-zumba-slug");
//            group.PhoneNumber.Should().Be("00000000000");
//            group.Email.Should().Be("hello@stockportzumba.whatever");
//            group.Website.Should().Be("stockportzumba.io");
//            group.Facebook.Should().Be("facebook.com/stockportzumba");
//            group.Address.Should().Be("zumba house,\nzumba road,\nzumba zumba zumba");
//            group.Description.Should().Be("The group description");
//        }

//        [Fact]
//        public async Task GetsShowcase()
//        {
//            //Arrange
//            const string slug = "showcase";
//            const string url = "url";

//            _mockUrlGenerator.Setup(o => o.UrlFor<Showcase>(slug, null)).Returns(url);
//            _mockHttpClient.Setup(o => o.Get(url, It.IsAny<Dictionary<string, string>>())).ReturnsAsync(new HttpResponse(200, JsonFileHelper.GetStringResponseFromFile("Showcase.json"), string.Empty));

//            //Act
//            var httpResponse = await _repository.Get<Showcase>(slug);
//            var showcase = httpResponse.Content as ProcessedShowcase;

//            //Assert
//            showcase.Title.Should().Be("test showcase");
//            showcase.Slug.Should().Be("test-showcase");
//            showcase.Teaser.Should().Be("Just a test");
//            showcase.Subheading.Should().Be("test subheading");
//            showcase.HeroImageUrl.Should().Be("heroImageUrl.jpg");
//            showcase.FeaturedItems.First().Title.Should().Be("test title");
//            showcase.Breadcrumbs.First().Title.Should().Be("test-title");
//        }  
//    }
//}

