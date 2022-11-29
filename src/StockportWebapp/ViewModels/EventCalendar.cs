using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using StockportWebapp.Validation;

namespace StockportWebapp.ViewModels
{
    public class EventCalendar
    {
        [Display(Name = "Start date")]
        [DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; }

        [Display(Name = "End date")]
        [DataType(DataType.Date)]
        [EndDateLaterThanStartDateValidation("DateFrom", "End date should be on or after the start date")]
        public DateTime? DateTo { get; set; }

        public string Category { get; set; }

        public string DateRange { get; set; }

        public List<Event> Events { get; private set; } = new List<Event>();
        public List<string> Categories { get; private set; } = new List<string>();
        public string Tag { get; set; }
        public string[] Price { get; set; }
        public string HomepageTags { get; set; }
        public IFilteredUrl FilteredUrl { get; private set; }
        public QueryUrl CurrentUrl { get; private set; }
        public Pagination Pagination { get; set; }

        public EventHomepage Homepage { get; set; }

        public EventCalendar() { }

        public bool FromSearch { get; set; }
        public string KeepTag { get; set; }
        public string Location { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

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

        public List<SelectListItem> CategoryOptions()
        {
            var result = new List<SelectListItem>();
            result.Add(new SelectListItem { Text = "All categories", Value = string.Empty });
            foreach (var cat in Categories)
            {
                result.Add(new SelectListItem { Text = cat, Value = cat });
            }

            return result;
        }

        public List<SelectListItem> EventCategoryOptions()
        {
            var result = new List<SelectListItem>();
            result.Add(new SelectListItem { Text = "All categories", Value = string.Empty });
            foreach (var cat in Homepage.Categories)
            {
                result.Add(new SelectListItem { Text = cat.Name, Value = cat.Slug });
            }

            return result;
        }

        public void AddCategories(List<string> categories)
        {
            Categories = categories;
        }

        public void AddFilteredUrl(IFilteredUrl filteredUrl)
        {
            FilteredUrl = filteredUrl;
        }

        public void AddQueryUrl(QueryUrl queryUrl)
        {
            CurrentUrl = queryUrl;
        }

        public string GetCustomEventFilterName()
        {
            return DateFrom.HasValue && DateTo.HasValue
                ? DateFrom.Value.ToString("dd/MM/yyyy") + " to " + DateTo.Value.ToString("dd/MM/yyyy")
                : string.Empty;
        }

        public RefineByBar RefineByBar()
        {
            var bar = new RefineByBar
            {
                ShowLocation = true,
                Filters = new List<RefineByFilters>()
            };

            if (!string.IsNullOrEmpty(KeepTag) || !string.IsNullOrEmpty(Tag))
            {
                var featured = new RefineByFilters
                {
                    Label = "Featured events",
                    Mandatory = false,
                    Name = "tag",
                    Items = new List<RefineByFilterItems>
                    {
                        new RefineByFilterItems { Label = KeepTag, Checked = !string.IsNullOrEmpty(Tag), Value = KeepTag }
                    }
                };

                bar.Filters.Add(featured);
            }

            var price = new RefineByFilters
            {
                Label = "Price",
                Mandatory = true,
                Name = "price",
                Items = new List<RefineByFilterItems>
                {
                    new RefineByFilterItems { Label = "Paid", Checked = Price == null || Price.Any(p => p == "paid"), Value = "paid" },
                    new RefineByFilterItems { Label = "Free", Checked = Price == null || Price.Any(p => p == "free"), Value = "free" }
                }
            };

            bar.Filters.Add(price);

            return bar;
        }
    }
}
