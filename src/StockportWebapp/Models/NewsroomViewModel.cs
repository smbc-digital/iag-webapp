using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Config;
using StockportWebapp.Utils;
using StockportWebapp.Validation;

namespace StockportWebapp.Models
{
    public class NewsroomViewModel
    {
        public string EmailAlertsUrl { get; private set; }
        public Newsroom Newsroom { get; private set; }

        // filters
        public string Tag { get; set; }
        public string Category { get; set; }
        public string DateRange { get; set; }

        [Required]
        [Display(Name = "Start date")]
        [DataType(DataType.Date)]
        [PastDateValidation]
        public DateTime? DateFrom { get; set; }

        [Required]
        [Display(Name = "End date")]
        [DataType(DataType.Date)]
        [PastDateValidation]
        [EndDateLaterThanStartDateValidation(otherPropertyName: "DateFrom", erroMessgae: "End date should be after the start date")]
        public DateTime? DateTo { get; set; }

        public List<string> Categories
        {
            get { return Newsroom?.Categories?.OrderBy(c => c).ToList(); }
        }

        public NewsroomViewModel() {}

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
            if (DateRange == "customdate" && DateFrom.HasValue && DateTo.HasValue)
            {
                // if Model.DateFrom && Model.DateTo have a value, dateFilter = its value as a string
                return DateFrom.Value.ToString("dd/MM/yyyy") + " to " + DateTo.Value.ToString("dd/MM/yyyy");
            }
            
            // if Model.DateFrom has a value, dateFilter = the month and year as a string ("MMMM yyyy")
            return DateFrom.HasValue ? DateFrom.Value.ToString("MMMM yyyy") : string.Empty;
        }
    }
}
