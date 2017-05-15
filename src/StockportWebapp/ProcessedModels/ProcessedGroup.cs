using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedGroup : IProcessedContentType
    {
        public readonly string Name;
        public readonly string Slug;
        public readonly string PhoneNumber;
        public readonly string Email;
        public readonly string Website;
        public readonly string Twitter;
        public readonly string Facebook;
        public readonly string Address;
        public readonly string Description;
        public readonly string ImageUrl;
        public readonly string ThumbnailImageUrl;
        public readonly List<GroupCategory> CategoriesReference;
        public List<Crumb> Breadcrumbs { get; }
        public readonly MapPosition MapPosition;
        public readonly bool Volunteering;

        public ProcessedGroup()
        {
        }

        public ProcessedGroup(string name, string slug, string phoneNumber, string email, string website, string twitter,
                      string facebook, string address, string description, string imageUrl, string thumbnailImageUrl, 
                      List<GroupCategory> categoriesReference, List<Crumb> breadcrumbs, MapPosition mapPosition, bool volunteering)
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
            Breadcrumbs = breadcrumbs;
            MapPosition = mapPosition;
            Volunteering = volunteering;
        }
    }
}