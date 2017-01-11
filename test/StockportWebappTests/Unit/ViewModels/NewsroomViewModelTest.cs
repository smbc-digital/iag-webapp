using System;
using System.Collections.Generic;
using FluentAssertions;
using StockportWebapp.Models;
using Xunit;

namespace StockportWebappTests.Unit.ViewModels
{
    public class NewsroomViewModelTest
    {
        private static readonly List<string> emptyList = new List<string>();

        private const string EmailAlertsUrl = "url";
        private const string Tag = "tag";
        private readonly Newsroom _newsroom;
        private readonly NewsroomViewModel _newsroomViewModel;
        private readonly List<Crumb> _breadcrumbs = new List<Crumb>();

        public NewsroomViewModelTest()
        {
            _newsroom = new Newsroom(new List<News>(), new List<Alert>(), true, "tag-id", new List<string>(), new List<DateTime>());
            _newsroomViewModel = new NewsroomViewModel(_newsroom, EmailAlertsUrl, "title", Tag, _breadcrumbs);
        }

        [Fact]
        public void ShouldSetEmailAlertsUrlWithTopicId()
        {
            var newsroomViewModel = new NewsroomViewModel(ANewsRoom(emailAlertsTopicId: "tag-id"), EmailAlertsUrl, "title", Tag, _breadcrumbs);

            newsroomViewModel.EmailAlertsUrl.Should().Be(string.Concat(EmailAlertsUrl, "?topic_id=", _newsroom.EmailAlertsTopicId));
        }

        [Fact]
        public void ShouldSetEmailAlertsUrlWithoutTopicId()
        {
            var newsroomViewModel = new NewsroomViewModel(ANewsRoom(emailAlertsTopicId: string.Empty), EmailAlertsUrl, "title", Tag, _breadcrumbs);

            newsroomViewModel.EmailAlertsUrl.Should().Be(EmailAlertsUrl);
        }

        [Fact]
        public void ShouldSetTitle()
        {
            _newsroomViewModel.Title.Should().Be("title");
        }

        [Fact]
        public void ShouldSetTag()
        {
            _newsroomViewModel.Tag.Should().Be(Tag);
        }

        [Fact]
        public void ShouldSetBreadcrumbs()
        {
            var breadcrumbs = new List<Crumb> { new Crumb("title", "slug", "type")};
            var newsroomViewModel = new NewsroomViewModel(ANewsRoom(), EmailAlertsUrl, "title", Tag, breadcrumbs);

            newsroomViewModel.Breadcrumbs.Should().HaveCount(1);
            newsroomViewModel.Breadcrumbs[0].Title.Should().Be("title");
        }

        [Fact]
        public void ShouldGiveCategoriesInAlphabeticalOrder()
        {
            var newsroom = ANewsRoom(categories: new List<string> { "Zebras", "Asses", "Oxen" });

            var newsroomViewModel = new NewsroomViewModel(newsroom, EmailAlertsUrl, "title", Tag, _breadcrumbs);

            var categories = newsroomViewModel.Categories;

            categories.Should().ContainInOrder("Asses", "Oxen", "Zebras");
        }

        private static Newsroom ANewsRoom(List<string> categories = null, string emailAlertsTopicId = "")
        {
            return new Newsroom(new List<News>(), new List<Alert>(), true, emailAlertsTopicId, categories ?? emptyList, new List<DateTime>());
        }
    }
}
