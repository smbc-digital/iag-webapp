using StockportWebapp.Models;
using System;
using System.Collections.Generic;

namespace StockportWebappTests_Unit.Builders
{
    internal class GroupBuilder
    {
        private string _name = "name";
        private string _metaDescription = "metaDescription";
        private string _slug = "slug";
        private string _phoneNumber = "01234567";
        private string _email = "notanemail@fake.email";
        private string _website = "http://www.stockport.gov.uk";
        private string _twitter = "twitteruser";
        private string _facebook = "facebookpage";
        private string _address = "address";
        private string _description = "description";
        private string _imageUrl = "imageurl";
        private string _thumbnailImageUrl = "thumbnail-image-url";
        private string _donationsText = "_donationsText";
        private string _donationsUrl = "_donationsUrl";
        private List<GroupCategory> _categoriesReference = new List<GroupCategory>();
        private List<GroupSubCategory> _subCategories = new List<GroupSubCategory>();
        private List<Crumb> _breadcrumbs = new List<Crumb>();
        private MapPosition _mapPosition = new MapPosition();
        private bool _volunteering = false;
        private List<Event> _events = new List<Event>();
        private GroupAdministrators _groupAdministrators = new GroupAdministrators
        {
            Items = new List<GroupAdministratorItems> { new GroupAdministratorItems { Email = "email@email.com", Permission = "A" } }
        };
        private DateTime? _dateHiddenFrom = null;
        private DateTime? _dateHiddenTo = null;
        private string _status = "status";
        private List<string> _cost = new List<string>();
        private string _costText = "cost text";
        private string _abilityLevel = "ability level";
        private bool _favourite = false;
        private string _volunteeringText = "volunteering text";
        private Organisation _organisation = new Organisation();
        private List<Group> _linkedGroups = new List<Group>();
        private bool _donations = false;
        private string _accessibleTransportLink = "http://www.link.link.link";
        private string _additionalInformation = "additional information";
        private List<Document> _additionalDocuments = new List<Document>();
        private DateTime? _dateLastModified = null;
        private List<string> _suitableFor = new List<string>();
        private List<string> _ageRange = new List<string>();
        private List<GroupBranding> _groupBranding = new List<GroupBranding>();
        private readonly List<Alert> _alerts = new List<Alert>();

        public Group Build()
        {
            return new Group(_name,
                    _slug,
                    _metaDescription,
                    _phoneNumber,
                    _email,
                    _website,
                    _twitter,
                    _facebook,
                    _address,
                    _description,
                    _imageUrl,
                    _thumbnailImageUrl,
                    _categoriesReference,
                    _subCategories,
                    _breadcrumbs,
                    _mapPosition,
                    _volunteering,
                    _events,
                    _groupAdministrators,
                    _dateHiddenFrom,
                    _dateHiddenTo,
                    _status,
                    _cost,
                    _costText,
                    _abilityLevel,
                    _favourite,
                    _volunteeringText,
                    _organisation,
                    _linkedGroups,
                    _donations,
                    _accessibleTransportLink,
                    _groupBranding,
                    _additionalInformation,
                    _additionalDocuments,
                    _dateLastModified,
                    _suitableFor,
                    _ageRange,
                    _donationsText,
                    _donationsUrl,
                    _alerts
                );
        }

        public GroupBuilder Image(string value)
        {
            _imageUrl = value;
            return this;
        }

        public GroupBuilder MetaDescription(string metaDescription)
        {
            _metaDescription = metaDescription;
            return this;
        }

        public GroupBuilder Email(string value)
        {
            _email = value;
            return this;
        }

        public GroupBuilder GroupAdministrators(GroupAdministrators value)
        {
            _groupAdministrators = value;
            return this;
        }

        public GroupBuilder Categories(List<GroupCategory> value)
        {
            _categoriesReference = value;
            return this;
        }

        public GroupBuilder Favourite(bool value)
        {
            _favourite = value;
            return this;
        }

        public GroupBuilder Slug(string value)
        {
            _slug = value;
            return this;
        }

        public GroupBuilder DateLastModified(DateTime value)
        {
            _dateLastModified = value;
            return this;
        }

        public GroupBuilder DonationsText(string donationsText)
        {
            _donationsText = donationsText;
            return this;
        }

        public GroupBuilder DonationsUrl(string donationsUrl)
        {
            _donationsUrl = donationsUrl;
            return this;
        }


    }
}