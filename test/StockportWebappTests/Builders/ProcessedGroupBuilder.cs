using System;
using System.Collections.Generic;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;

namespace StockportWebappTests.Builders
{
    internal class ProcessedGroupBuilder
    {
        private string _name = "name";
        private string _slug = "slug";
        private string _phoneNumber = "01234567";
        private string _email = "notanemail@fake.email";
        private string _website = "http://www.stockport.gov.uk";
        private string _twitter = "twitteruser";
        private string _facebook = "facebookpage";
        private string _address = "address";
        private string _description = "description";
        private string _imageUrl = "imageurl";
        private string _donationsText = "_donationsText";
        private string _donationsUrl = "_donationsUrl";
        private string _thumbnailImageUrl = "thumbnail-image-url";
        private List<GroupCategory> _categoriesReference = new List<GroupCategory>();
        private List<GroupSubCategory> _subCategories = new List<GroupSubCategory>();
        private List<Crumb> _breadcrumbs = new List<Crumb>();
        private MapDetails _mapDetails = new MapDetails();
        private Volunteering _volunteering = new Volunteering();
        private List<Event> _events = new List<Event>();
        private GroupAdministrators _groupAdministrators = new GroupAdministrators
        {
            Items = new List<GroupAdministratorItems> { new GroupAdministratorItems { Email = "email@email.com", Permission = "A" } }
        };
        private DateTime? _dateHiddenFrom = null;
        private DateTime? _dateHiddenTo = null;
        private List<string> _cost = new List<string>();
        private string _costText = "cost text";
        private string _abilityLevel = "ability level";
        private bool _favourite = false;
        private Organisation _organisation = new Organisation();
        private List<Group> _linkedGroups = new List<Group>();
        private Donations _donations = new Donations();
        private string _additionalInformation = "additional information";
        private List<Document> _additionalDocuments = new List<Document>();

        public ProcessedGroup Build()
        {
            return new ProcessedGroup(_name,
                _slug,
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
                _events,
                _groupAdministrators,
                _dateHiddenFrom,
                _dateHiddenTo,
                _cost,
                _costText,
                _abilityLevel,
                _favourite,
                _volunteering,
                _organisation,
                _linkedGroups,
                _donations,
                _mapDetails,
                _additionalInformation,
                _additionalDocuments,
                _donationsText,
                _donationsUrl
            );
        }

        public ProcessedGroupBuilder Email(string value)
        {
            _email = value;
            return this;
        }

        public ProcessedGroupBuilder GroupAdministrators(GroupAdministrators value)
        {
            _groupAdministrators = value;
            return this;
        }

        public ProcessedGroupBuilder MapDetails(MapDetails value)
        {
            _mapDetails = value;
            return this;
        }
    }
}
