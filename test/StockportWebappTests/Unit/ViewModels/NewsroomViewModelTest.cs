using System;
using System.Collections.Generic;
using FluentAssertions;
using StockportWebapp.Models;
using Xunit;

namespace StockportWebappTests.Unit.ViewModels
{
    public class NewsroomViewModelTest
    {
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
            _newsroomViewModel.EmailAlertsUrl.Should().Be(string.Concat(EmailAlertsUrl, "?topic_id=", _newsroom.EmailAlertsTopicId));
        }

        [Fact]
        public void ShouldSetEmailAlertsUrlWithoutTopicId()
        {
            var newsroom = new Newsroom(new List<News>(), new List<Alert>(), true, string.Empty, new List<string>(), new List<DateTime>());
            var newsroomViewModel = new NewsroomViewModel(newsroom, EmailAlertsUrl, "title", Tag, _breadcrumbs);

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
            var newsroomViewModel = new NewsroomViewModel(_newsroom, EmailAlertsUrl, "title", Tag, breadcrumbs);

            newsroomViewModel.Breadcrumbs.Should().HaveCount(1);
            newsroomViewModel.Breadcrumbs[0].Title.Should().Be("title");
        }

        [Fact]
        public void ShouldGiveCategoriesInAlphabeticalOrder()
        {
            var unorderedCategories = new List<string>() { "Zebras", "Asses", "Oxen" };
            var newsroom = new Newsroom(new List<News>(), new List<Alert>(), true, "tag-id", unorderedCategories, new List<DateTime>());
            var newsroomViewModel = new NewsroomViewModel(newsroom, EmailAlertsUrl, "title", Tag, _breadcrumbs);

            var categories = newsroomViewModel.Categories;

            categories.Should().ContainInOrder("Asses", "Oxen", "Zebras");
        }
    }
}
