﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using StockportWebapp.Config;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using StockportWebapp.Validation;

namespace StockportWebapp.ViewModels
{
    public class NewsroomViewModel
    {
        // data
        public string EmailAlertsUrl { get; private set; }
        public Newsroom Newsroom { get; private set; }
        public List<string> Categories { get { return Newsroom?.Categories?.OrderBy(c => c).ToList(); } }

        // filters
        public string Tag { get; set; }
        public string Category { get; set; }
        public string DateRange { get; set; }

        // form elements
        [Required]
        [Display(Name = "Start date")]
        [DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; }

        [Required]
        [Display(Name = "End date")]
        [DataType(DataType.Date)]
        [EndDateLaterThanStartDateValidation("DateFrom", "End date should be on or after the start date")]
        public DateTime? DateTo { get; set; }

        // urls
        public IFilteredUrl FilteredUrl { get; private set; }
        public QueryUrl CurrentUrl { get; private set; }
        public Pagination Pagination { get; set; }

        public NewsroomViewModel() { }

        public NewsroomViewModel(Newsroom newsroom, string emailAlertsUrl)
        {
            Newsroom = newsroom;
            EmailAlertsUrl = SetEmailAlertsUrlWithTopicId(newsroom, emailAlertsUrl);
        }

        private static string SetEmailAlertsUrlWithTopicId(Newsroom newsroom, string url)
        {
            return !string.IsNullOrEmpty(newsroom.EmailAlertsTopicId) ? string.Concat(url, "?topic_id=", newsroom.EmailAlertsTopicId) : url;
        }

        internal void AddNews(Newsroom newsRoom)
        {
            Newsroom = newsRoom;
        }

        internal void AddUrlSetting(AppSetting urlSetting)
        {
            EmailAlertsUrl = urlSetting.ToString();
        }

        public string GetActiveDateFilter()
        {
            if (!DateFrom.HasValue || !DateTo.HasValue) return string.Empty;

            if (DateRange == "customdate" && DateFrom.Value == DateTo.Value) return DateFrom.Value.ToString("dd/MM/yyyy");

            if (DateRange == "customdate") return DateFrom.Value.ToString("dd/MM/yyyy") + " to " + DateTo.Value.ToString("dd/MM/yyyy");

            return DateFrom.Value.ToString("MMMM yyyy");
        }

        public void AddFilteredUrl(IFilteredUrl filteredUrl)
        {
            FilteredUrl = filteredUrl;
        }

        public void AddQueryUrl(QueryUrl queryUrl)
        {
            CurrentUrl = queryUrl;
        }
    }
}
