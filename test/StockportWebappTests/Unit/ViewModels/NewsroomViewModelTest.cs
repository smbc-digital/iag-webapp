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
        private readonly Newsroom _newsroom;

        public NewsroomViewModelTest()
        {
            _newsroom = new Newsroom(new List<News>(), new List<Alert>(), true, "tag-id", new List<string>(), new List<DateTime>());
        }

        [Fact]
        public void ShouldSetEmailAlertsUrlWithTopicId()
        {
            var newsroomViewModel = new NewsroomViewModel(BuildNewsRoom(emailAlertsTopicId: "tag-id"), EmailAlertsUrl);

            newsroomViewModel.EmailAlertsUrl.Should().Be(string.Concat(EmailAlertsUrl, "?topic_id=", _newsroom.EmailAlertsTopicId));
        }

        [Fact]
        public void ShouldSetEmailAlertsUrlWithoutTopicId()
        {
            var newsroomViewModel = new NewsroomViewModel(BuildNewsRoom(emailAlertsTopicId: string.Empty), EmailAlertsUrl);

            newsroomViewModel.EmailAlertsUrl.Should().Be(EmailAlertsUrl);
        }

        [Fact]
        public void ShouldGiveCategoriesInAlphabeticalOrder()
        {
            var newsroom = BuildNewsRoom(categories: new List<string> { "Zebras", "Asses", "Oxen" });

            var newsroomViewModel = new NewsroomViewModel(newsroom, EmailAlertsUrl);

            var categories = newsroomViewModel.Categories;

            categories.Should().ContainInOrder("Asses", "Oxen", "Zebras");
        }

        private static Newsroom BuildNewsRoom(List<string> categories = null, string emailAlertsTopicId = "")
        {
            return new Newsroom(new List<News>(), new List<Alert>(), true, emailAlertsTopicId, categories ?? emptyList, new List<DateTime>());
        }
    }
}
