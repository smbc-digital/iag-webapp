using System;
using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class Group
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbnailImageUrl { get; set; }
        public List<GroupCategory> CategoriesReference { get; set; }
        public List<GroupSubCategory> SubCategories { get; set; }
        public List<Crumb> Breadcrumbs { get; set; }
        public MapPosition MapPosition { get; set; }
        public bool Volunteering { get; set; }
        public List<Event> Events { get; set; }
        public GroupAdministrators GroupAdministrators { get; set; }
        public DateTime? DateHiddenFrom { get; set; }
        public DateTime? DateHiddenTo { get; set; }
        public string Status { get; set; }
        public string Cost { get; set; }
        public string CostText { get; set; }
        public string AbilityLevel { get; set; }
        public bool Favourite { get; set; }
        public string VolunteeringText { get; set; }
        public Organisation Organisation { get; set; }
        public List<Group> LinkedGroups { get; private set; }

        public Group(string name, string slug, string phoneNumber, string email, string website,
            string twitter, string facebook, string address, string description, string imageUrl,
            string thumbnailImageUrl, List<GroupCategory> categoriesReference, List<GroupSubCategory> subCategories, List<Crumb> breadcrumbs,
            MapPosition mapPosition, bool volunteering, List<Event> events, GroupAdministrators groupAdministrators, DateTime? dateHiddenFrom, 
            DateTime? dateHiddenTo, string status, string cost, string costText, string abilityLevel, bool favourite, string volunteeringText,
            Organisation organisation, List<Group> linkedGroups)
        {
            Name = name;
            Slug = slug;
            PhoneNumber = phoneNumber;
            Email = email;
            Website = website;
            Twitter = twitter;
            Facebook = facebook;
            Address = address;
            Description = description;
            ImageUrl = imageUrl;
            ThumbnailImageUrl = thumbnailImageUrl;
            CategoriesReference = categoriesReference;
            SubCategories = subCategories;
            Breadcrumbs = breadcrumbs;
            MapPosition = mapPosition;
            Volunteering = volunteering;
            Events = events;
            GroupAdministrators = groupAdministrators;
            DateHiddenFrom = dateHiddenFrom;
            DateHiddenTo = dateHiddenTo;
            Status = status;
            Cost = cost;
            CostText = costText;
            AbilityLevel = abilityLevel;
            Favourite = favourite;
            VolunteeringText = volunteeringText;
            Organisation = organisation;
            LinkedGroups = linkedGroups;
        }
    }
}