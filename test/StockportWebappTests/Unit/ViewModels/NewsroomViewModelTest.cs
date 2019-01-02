using System;
using System.Collections.Generic;
using FluentAssertions;
using StockportWebapp.Models;
using StockportWebapp.ViewModels;
using Xunit;

namespace StockportWebappTests_Unit.Unit.ViewModels
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

        [Fact]
        public void ShouldReturnToAndFromDatesIfDateRangeIsCustomDate()
        {
            var newsroomViewModel = new NewsroomViewModel() {DateRange = "customdate", DateFrom = new DateTime(2016, 01, 01), DateTo = new DateTime(2016, 02, 01)};

            var result = newsroomViewModel.GetActiveDateFilter();

            result.Should().Be("01/01/2016 to 01/02/2016");
        }

        [Fact]
        public void ShouldReturnMonthNameIfDateRangeIsMonth()
        {
            var newsroomViewModel = new NewsroomViewModel() { DateFrom = new DateTime(2016, 01, 01), DateTo = new DateTime(2016, 01, 31) };

            var result = newsroomViewModel.GetActiveDateFilter();

            result.Should().Be("January 2016");
        }

        [Fact]
        public void ShouldDisplaySingleDateIfToDateAndFromDateAreTheSame()
        {
            var newsroomViewModel = new NewsroomViewModel { DateRange = "customdate", DateFrom = new DateTime(2016, 01, 01), DateTo = new DateTime(2016, 01, 01) };

            var result = newsroomViewModel.GetActiveDateFilter();

            result.Should().Be("01/01/2016");
        }

        private static Newsroom BuildNewsRoom(List<string> categories = null, string emailAlertsTopicId = "")
        {
            return new Newsroom(new List<News>(), new List<Alert>(), true, emailAlertsTopicId, categories ?? emptyList, new List<DateTime>());
        }
    }
}
