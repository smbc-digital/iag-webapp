using StockportWebapp.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace StockportWebapp.Models
{
    public class EventCalendar
    {
        [Required]
        [Display(Name = "Start date")]
        [DataType(DataType.Date)]
        public DateTime? datefrom { get; set; }

        [Required]
        [Display(Name = "End date")]
        [DataType(DataType.Date)]
        [EndDateLaterThanStartDateValidation(otherPropertyName: "datefrom", erroMessgae: "End date should be on or after the start date")]
        public DateTime? dateto { get; set; }

        public string category { get; set; }

        public string DateRange { get; set; }

        public List<Event> Events { get; private set; } = new List<Event>();
        public List<string> Categories { get; private set; } = new List<string>();
        public string Tag { get; set; }

        public EventCalendar() { }

        public EventCalendar(List<Event> events, List<string> categories)
        {
            Events = events;
            Categories = categories;
        }

        public bool DoesCategoryExist(string categoryItem)
        {
            return Categories.Contains(categoryItem);
        }

        public void AddEvents(List<Event> events)
        {
            Events = events;
        }

        public void AddCategories(List<string> categories)
        {
            Categories = categories;
        }
    }
}
