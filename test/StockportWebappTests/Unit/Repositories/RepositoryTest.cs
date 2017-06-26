using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Utils;
using StockportWebapp.ViewModels;
using Xunit;

namespace StockportWebappTests.Unit.Repositories
{
    public class RepositoryTest
    {
        private readonly Repository _repository;
        private readonly Mock<IHttpClient> _httpClientMock = new Mock<IHttpClient>();
        private readonly UrlGenerator _urlGenerator;

        public RepositoryTest()
        {
            var appConfig = new Mock<IApplicationConfiguration>();
            appConfig.Setup(o => o.GetContentApiUri()).Returns(new Uri("http://localhost:5000/"));

            _urlGenerator = new UrlGenerator(appConfig.Object, new BusinessId(""));
            _repository = new Repository(_urlGenerator, _httpClientMock.Object);
        }

        [Fact]
        public void GetsNotFoundForTopicNotFound()
        {
            const string unrecognizedTopic = "not found";
            const string topicNotFoundError = "No topic found for not found";
            var url = _urlGenerator.UrlFor<Topic>(unrecognizedTopic);

            _httpClientMock.Setup(o => o.Get(url)).ReturnsAsync(HttpResponse.Failure(404, topicNotFoundError));

            var httpResponse = AsyncTestHelper.Resolve(_repository.Get<Topic>(unrecognizedTopic));

            httpResponse.StatusCode.Should().Be(404);
            httpResponse.Error.Should().Be("No topic found for not found");
        }

        [Fact]
        public void GetsGroupsThatAnEmailAdministrors()
        {
            const string email = "buggs@loonytunes.com";
            var url = _urlGenerator.AdministratorsGroups(email);

            _httpClientMock.Setup(o => o.Get(url))
                .ReturnsAsync(new HttpResponse(
                    200,
                    File.ReadAllText("Unit/MockResponses/GroupListing.json"),
                    string.Empty));

            var httpResponse = AsyncTestHelper.Resolve(_repository.GetAdministratorsGroups(email));
            var groups = httpResponse.Content as List<Group>;

            groups.Count(x => x.GroupAdministrators.Items.Any(item => item.Email == email)).Should().Be(1);
        }

        [Fact]
        public void GetsTopicByTopicSlug()
        {
            const string topicSlug = "healthy-living";
            var url = _urlGenerator.UrlFor<Topic>(topicSlug);

            _httpClientMock.Setup(o => o.Get(url))
                .ReturnsAsync(new HttpResponse(
                    200,
                    File.ReadAllText("Unit/MockResponses/Topic.json"),
                    string.Empty));

            var httpResponse = AsyncTestHelper.Resolve(_repository.Get<Topic>(topicSlug));
            var topic = httpResponse.Content as Topic;

            topic.Name.Should().Be("Healthy Living");
            topic.Slug.Should().Be(topicSlug);
            topic.Summary.Should().Be(MarkdownWrapper.ToHtml("Summary content"));
            topic.BackgroundImage.Should().Be("image.jpg");
            topic.Breadcrumbs.Should().HaveCount(2);
            topic.Breadcrumbs.First().Title.Should().Be("Healthy Living");
            topic.Breadcrumbs.First().NavigationLink.Should().Be("/topic/healthy-living");
            topic.NavigationLink.Should().Be("/topic/healthy-living");
            topic.EmailAlerts.Should().Be(true);
            topic.EmailAlertsTopicId.Should().Be("test-id");
        }

        [Fact]
        public void ShouldGetTopicWithSevenSubItemsAndOneSecondaryItem()
        {
            const string topicSlug = "healthy-living";
            var url = _urlGenerator.UrlFor<Topic>(topicSlug);

            _httpClientMock.Setup(o => o.Get(url))
                .ReturnsAsync(new HttpResponse(
                    200,
                    File.ReadAllText("Unit/MockResponses/TopicWithSubItemsAndSecondaryItems.json"),
                    string.Empty));

            var httpResponse = AsyncTestHelper.Resolve(_repository.Get<Topic>(topicSlug));
            var topic = httpResponse.Content as Topic;

            topic.SubItems.Should().HaveCount(7);
            topic.SecondaryItems.Should().HaveCount(1);
            topic.EmailAlerts.Should().Be(true);
            topic.EmailAlertsTopicId.Should().Be("test-id");
        }


        [Fact]
        public void GetsTopigBySlugWithAlertsAndSubItems()
        {
            const string topicSlug = "healthy-living";
            var url = _urlGenerator.UrlFor<Topic>(topicSlug);

            _httpClientMock.Setup(o => o.Get(url))
                .ReturnsAsync(new HttpResponse(
                    200,
                    File.ReadAllText("Unit/MockResponses/TopicWithAlerts.json"),
                    string.Empty));

            var httpResponse = AsyncTestHelper.Resolve(_repository.Get<Topic>(topicSlug));
            var topic = httpResponse.Content as Topic;

            topic.SubItems.Should().HaveCount(1);
            topic.SubItems.FirstOrDefault().Title.Should().Be("Getting Support");

            topic.Alerts.Should().HaveCount(1);
            topic.Alerts.FirstOrDefault().Title.Should().Be("This is an alert");
            topic.Alerts.FirstOrDefault().SubHeading.Should().Be("It has a subheading");
            topic.Alerts.FirstOrDefault()
                .Body.Should()
                .Be("<p>It also has a body text</p>\n");
            topic.Alerts.FirstOrDefault().Severity.Should().Be(Severity.Warning);

            topic.EmailAlerts.Should().Be(true);
            topic.EmailAlertsTopicId.Should().Be("test-id");
        }

        [Fact]
        public void ShouldGetStartPage()
        {
            var startPageSlug = "slug";
            var url = _urlGenerator.UrlFor<StartPage>(startPageSlug);

            _httpClientMock.Setup(o => o.Get(url))
                .ReturnsAsync(new HttpResponse(200, File.ReadAllText("Unit/MockResponses/StartPage.json"), string.Empty));

            var httpResponse = AsyncTestHelper.Resolve(_repository.Get<StartPage>(startPageSlug));
            var startPage = httpResponse.Content as StartPage;

            startPage.Title.Should().Be("Start Page");
            startPage.Slug.Should().Be("start-page");
            startPage.Teaser.Should().Be("this is a teaser");
            startPage.Summary.Should().Be("This is a summary ");
            startPage.UpperBody.Should().Be(MarkdownWrapper.ToHtml("An upper body"));
            startPage.FormLinkLabel.Should().Be("Start now");
            startPage.FormLink.Should().Be("http://start.com");
            startPage.LowerBody.Should().Be(MarkdownWrapper.ToHtml("Lower body"));
            startPage.Breadcrumbs.Should().HaveCount(1);
            startPage.Breadcrumbs.First().NavigationLink.Should().Be("/topic/sub-topic3");
            startPage.Icon.Should().Be("icon");
            startPage.BackgroundImage.Should().Be("image.jpg");
            startPage.Alerts.Should().HaveCount(1);
            startPage.Alerts.First().Title.Should().Be("New alert");
            startPage.Alerts.First().SubHeading.Should().Be("alert sub heading updated");
            startPage.Alerts.First().Body.Should().Contain("Alert body");
            startPage.Alerts.First().Severity.Should().Be(Severity.Error);
        }

        [Fact]
        public void GetsNotFoundForStartPageNotFound()
        {
            var nonExistentStartPage = "startPageNotFound";
            var articleNotFoundError = "No start-page found for startPageNotFound";

            var url = _urlGenerator.UrlFor<StartPage>(nonExistentStartPage);

            _httpClientMock.Setup(o => o.Get(url))
                .ReturnsAsync(HttpResponse.Failure(404, articleNotFoundError));
            var httpResponse = AsyncTestHelper.Resolve(_repository.Get<StartPage>(nonExistentStartPage));

           httpResponse.StatusCode.Should().Be(404);
           httpResponse.Error.Should().Be("No start-page found for startPageNotFound");
        }

        /*
         * Homepage
         */
        [Fact]
        public void GetsHomepage()
        {
            var url = _urlGenerator.UrlFor<Homepage>();

            _httpClientMock.Setup(o => o.Get(url))
                .ReturnsAsync(new HttpResponse(200, File.ReadAllText("Unit/MockResponses/HomepageStockportGov.json"),
                    string.Empty));

            var httpResponse = AsyncTestHelper.Resolve(_repository.Get<Homepage>());
            var homepage = httpResponse.Content as Homepage;

            homepage.FeaturedTasksHeading.Should().Be("Featured tasks heading");
            homepage.FeaturedTasksSummary.Should().Be("Featured tasks summary");

            //Featured Tasks
            homepage.FeaturedTasks.Should().HaveCount(6);
            homepage.FeaturedTasks.First().Title.Should().Be("Pay Council Tax");
            homepage.FeaturedTasks.First().NavigationLink.Should().Be("/start/pay-council-tax");
            homepage.FeaturedTasks.First().Icon.Should().Be("fa fa-home");
            homepage.FeaturedTasks.Last().Title.Should().Be("Search job vacancies");

            //FeaturedTopcs
            homepage.FeaturedTopics.Should().HaveCount(2);
            homepage.FeaturedTopics.First().Slug.Should().Be("council-tax");
            homepage.FeaturedTopics.First().Name.Should().Be("Council Tax");
            homepage.FeaturedTopics.First().Teaser.Should().Be("How to pay, discounts");

            // alerts
            homepage.Alerts.First().Title.Should().Be("Warning alert");
            homepage.Alerts.First().SubHeading.Should().Be("This is a warning alert.");
            homepage.Alerts.First().Body.Should().Be(MarkdownWrapper.ToHtml("This is a warning alert."));
            homepage.Alerts.First().Severity.Should().Be(Severity.Warning);

            //freeText
            homepage.FreeText.Should().Be("homepage text");
        }

        [Fact]
        public void GetsNewsListing()
        {
            var url = _urlGenerator.UrlFor<Newsroom>();

            _httpClientMock.Setup(o => o.Get(url))
                .ReturnsAsync(new HttpResponse(200, File.ReadAllText("Unit/MockResponses/Newsroom.json"), string.Empty));

            var httpResponse = AsyncTestHelper.Resolve(_repository.Get<Newsroom>());
            var news = httpResponse.Content as Newsroom;

            news.News.Count.Should().Be(2);

            var firstNewsItem = news.News[0];
            firstNewsItem.Title.Should().Be("News 26th Aug");
            firstNewsItem.Slug.Should().Be("news-26th-aug");
            firstNewsItem.Teaser.Should().Be("test");
            firstNewsItem.Image.Should().Be("");
            firstNewsItem.ThumbnailImage.Should().Be("");
            firstNewsItem.Body.Should().Be("test");
            firstNewsItem.Breadcrumbs.Should().HaveCount(1);
            var firstNewsItemCrumb = firstNewsItem.Breadcrumbs.First();
            firstNewsItemCrumb.Title.Should().Be("News");
            firstNewsItemCrumb.NavigationLink.Should().Be("/news");
            var firstNewsItemAlerts = firstNewsItem.Alerts;
            firstNewsItemAlerts.Count.Should().Be(1);
            firstNewsItemAlerts.First().Title.Should().Be("New alert");
            firstNewsItemAlerts.First().SubHeading.Should().Be("alert sub heading updated");
            firstNewsItemAlerts.First().Body.Should().Contain("Alert body");
            firstNewsItemAlerts.First().Severity.Should().Be(Severity.Error);

            var secondNewsItem = news.News[1];
            secondNewsItem.Title.Should().Be("Another news article");
            secondNewsItem.Slug.Should().Be("another-news-article");
            secondNewsItem.Teaser.Should().Be("This is another news article");
            secondNewsItem.Image.Should().Be("image.jpg");
            secondNewsItem.ThumbnailImage.Should().Be("thumbnail.jpg");
            secondNewsItem.Body.Should().Be("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam gravida eu mauris in consectetur. Nullam nulla urna, sagittis a ex sit amet, ultricies rhoncus mauris. Quisque vel placerat turpis, vitae consectetur mauris.");      
            var secondNewsItemCrumb = secondNewsItem.Breadcrumbs.First();
            secondNewsItemCrumb.Title.Should().Be("News");
            secondNewsItemCrumb.NavigationLink.Should().Be("/news");
            var secondNewsItemAlerts = secondNewsItem.Alerts;
            secondNewsItemAlerts.Count.Should().Be(0);

            news.Alerts.Count.Should().Be(2);
            news.Alerts.First().Title.Should().Be("New alert");
            news.Alerts.First().SubHeading.Should().Be("alert sub heading updated");
            news.Alerts.First().Body.Should().Be("<p>Alert body</p>\n");
            news.Alerts.First().Severity.Should().Be("Error");

            news.EmailAlerts.Should().Be(true);
            news.EmailAlertsTopicId.Should().Be("test-id");
        }

        [Fact]
        public void GetsAtoZListingForTheLetterV()
        {
            var url = _urlGenerator.UrlFor<List<AtoZ>>();

            _httpClientMock.Setup(o => o.Get(url))
                .ReturnsAsync(new HttpResponse(200, File.ReadAllText("Unit/MockResponses/AtoZ.json"), string.Empty));

            var httpResponse = AsyncTestHelper.Resolve(_repository.Get<List<AtoZ>>());
            var atozListing = httpResponse.Content as List<AtoZ>;

            atozListing.Count.Should().Be(1);
            var firstItem = atozListing[0];

            firstItem.Title.Should().Be("Vintage Village turns 6 years old");
            firstItem.Teaser.Should().Be("The vintage village turned 6 with a great reception");
            firstItem.NavigationLink.Should().Be("/vintage-village-turns-6-years-old");
        }

        [Fact]
        public void GetsLatestNewsListing()
        {
            var url = _urlGenerator.UrlFor<Newsroom>("2");

            _httpClientMock.Setup(o => o.Get(url))
                .ReturnsAsync(new HttpResponse(200, File.ReadAllText("Unit/MockResponses/Newsroom.json"), string.Empty));

            var httpResponse = AsyncTestHelper.Resolve(_repository.Get<Newsroom>("2"));
            var news = httpResponse.Content as Newsroom;

            news.News.Count.Should().Be(2);
            news.News.First().Title.Should().Be("News 26th Aug");
            news.News.First().Slug.Should().Be("news-26th-aug");
            news.News.First().Teaser.Should().Be("test");
        }

        [Fact]
        public void GetsLatestNewsListingForTag()
        {
            var url = _urlGenerator.UrlFor<Newsroom>("/tag/Tag1");

            _httpClientMock.Setup(o => o.Get(url))
                .ReturnsAsync(new HttpResponse(200, File.ReadAllText("Unit/MockResponses/Newsroom.json"), string.Empty));

            var httpResponse = AsyncTestHelper.Resolve(_repository.Get<Newsroom>("/tag/Tag1"));
            var news = httpResponse.Content as Newsroom;

            news.News.Count.Should().Be(2);
            news.News.First().Title.Should().Be("News 26th Aug");
            news.News.First().Slug.Should().Be("news-26th-aug");
            news.News.First().Teaser.Should().Be("test");

            news.News.First().Tags.Count.Should().Be(2);
            news.News.First().Tags.First().Should().Be("Tag1");

            news.EmailAlerts.Should().Be(true);
            news.EmailAlertsTopicId.Should().Be("test-id");
        }

        [Fact]
        public void GetsRedirects()
        {
            var url = _urlGenerator.RedirectUrl();

            _httpClientMock.Setup(o => o.Get(url))
                .ReturnsAsync(new HttpResponse(200, File.ReadAllText("Unit/MockResponses/Redirects.json"), string.Empty));

            var httpResponse = AsyncTestHelper.Resolve(_repository.GetRedirects());
            var redirects = httpResponse.Content as Redirects;

            const string businessId = "unittest";
            var shortUrlRedirects = redirects.ShortUrlRedirects;
            shortUrlRedirects.Count.Should().Be(1);
            shortUrlRedirects.Should().ContainKey(businessId);
            shortUrlRedirects[businessId].Count.Should().Be(2);
            shortUrlRedirects[businessId].ContainsKey("/this-is-another-article").Should().BeTrue();
            shortUrlRedirects[businessId].ContainsKey("/test").Should().BeTrue();

            var legacyUrlRedirects = redirects.LegacyUrlRedirects;
            legacyUrlRedirects.Count.Should().Be(1);
            legacyUrlRedirects.Should().ContainKey(businessId);
            legacyUrlRedirects[businessId].Count.Should().Be(1);
            legacyUrlRedirects[businessId].ContainsKey("/this-is-a-redirect-from").Should().BeTrue();
        }

        [Fact]
        public void GetsEventCalendar()
        {
            var url = _urlGenerator.UrlFor<EventCalendar>();

            _httpClientMock.Setup(o => o.Get(url))
                .ReturnsAsync(new HttpResponse(200, File.ReadAllText("Unit/MockResponses/EventsCalendar.json"), string.Empty));

            var httpResponse = AsyncTestHelper.Resolve(_repository.Get<EventCalendar>());
            var events = httpResponse.Content as EventCalendar;

            events.Events.Count.Should().Be(3);
            events.Events.First().Title.Should().Be("This is the event");
            events.Events.First().Slug.Should().Be("event-of-the-century");
            events.Events.First().Teaser.Should().Be("Read more for the event");
        }

        [Fact]
        public void GetsEventCalendarBySlug()
        {
            var url = _urlGenerator.UrlFor<Event>("event-of-the-century");

            _httpClientMock.Setup(o => o.Get(url))
                .ReturnsAsync(new HttpResponse(200, File.ReadAllText("Unit/MockResponses/Event.json"), string.Empty));

            var httpResponse = AsyncTestHelper.Resolve(_repository.Get<Event>("event-of-the-century"));
            var eventList = httpResponse.Content as Event;

            eventList.Title.Should().Be("This is the event");
            eventList.Slug.Should().Be("event-of-the-century");
            eventList.Teaser.Should().Be("Read more for the event");
        }

        [Fact]
        public void GetsLatestEventsByLimit()
        {
            var url = _urlGenerator.UrlForLimit<EventCalendar>(2);

            _httpClientMock.Setup(o => o.Get(url))
                .ReturnsAsync(new HttpResponse(200, File.ReadAllText("Unit/MockResponses/EventListing.json"),
                    string.Empty));

            var httpResponse = AsyncTestHelper.Resolve(_repository.GetLatest<EventCalendar>(2));
            var eventList = httpResponse.Content as EventCalendar;

            eventList.Events.Should().HaveCount(2);
            eventList.Events.First().Title.Should().Be("This is the event");
            eventList.Events.First().Slug.Should().Be("event-of-the-century");
            eventList.Events.First().Teaser.Should().Be("Read more for the event");
        }

    }
}