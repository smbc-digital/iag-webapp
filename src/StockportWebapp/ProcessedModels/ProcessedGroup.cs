using System;
using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedGroup : IProcessedContentType
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string MetaDescription { get; set; }
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
        public List<Event> Events { get; set; }
        public GroupAdministrators GroupAdministrators { get; set; }
        public DateTime? DateHiddenFrom { get; set; }
        public DateTime? DateHiddenTo { get; set; }
        public DateTime? DateLastModified { get; set; }
        public List<string> Cost { get; set; }
        public string CostText { get; set; }
        public string AbilityLevel { get; set; }
        public bool Favourite { get; set; }
        public Volunteering Volunteering { get; set; }
        public Organisation Organisation { get; set; }
        public List<Group> LinkedGroups { get; private set; }
        public Donations Donations { get; set; }
        public MapDetails MapDetails { get; set; }
        public string CurrentUrl { get; private set; }
        public string AdditionalInformation { get; set; }
        public List<Document> AdditionalDocuments { get; set; }
        public string DonationsText { get; set; }
        public string DonationsUrl { get; set; }
        public List<GroupBranding> GroupBranding { get; set; }


        public ProcessedGroup()
        {
        }

        public ProcessedGroup(string name, string slug, string metaDescription, string phoneNumber, string email, string website, string twitter,
                      string facebook, string address, string description, string imageUrl, string thumbnailImageUrl, List<GroupCategory> categoriesReference, List<GroupSubCategory> subCategories,
                      List<Crumb> breadcrumbs, List<Event> events, GroupAdministrators groupAdministrators, DateTime? dateHiddenFrom, DateTime? dateHiddenTo,
                        List<string> cost, string costText, string abilityLevel, bool favourite, Volunteering volunteering, Organisation organisation, List<Group> linkedGroups, Donations donations,
                     MapDetails mapDetails, string additionalInformation, List<Document> additionalDocuments, string donationsText, string donationsUrl, DateTime? dateLastModified, List<GroupBranding> groupBranding)
        {
            Name = name;
            Slug = slug;
            MetaDescription = metaDescription;
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
            MapDetails = mapDetails;
            AdditionalInformation = additionalInformation;
            Volunteering = volunteering;
            Events = events;
            GroupAdministrators = groupAdministrators;
            DateHiddenFrom = dateHiddenFrom;
            DateHiddenTo = dateHiddenTo;
            Cost = cost;
            CostText = costText;
            AbilityLevel = abilityLevel;
            Favourite = favourite;
            Organisation = organisation;
            LinkedGroups = linkedGroups;
            Donations = donations;
            AdditionalInformation = additionalInformation;
            AdditionalDocuments = additionalDocuments;
            DonationsText = donationsText;
            DonationsUrl = donationsUrl;
            DateLastModified = dateLastModified;
            GroupBranding = groupBranding;
        }

        public void SetCurrentUrl(string url)
        {
            this.CurrentUrl = url;
        }
    }
}