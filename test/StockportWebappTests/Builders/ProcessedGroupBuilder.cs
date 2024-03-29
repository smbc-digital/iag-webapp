﻿namespace StockportWebappTests_Unit.Builders;

internal class ProcessedGroupBuilder
{
    private string _name = "name";
    private string _slug = "slug";
    private string _metaDescription = "metaDescription";
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
    private DateTime? _dateLastModified = null;
    private List<string> _cost = new List<string>();
    private string _costText = "cost text";
    private string _abilityLevel = "ability level";
    private bool _favourite = false;
    private Organisation _organisation = new Organisation();
    private List<Group> _linkedGroups = new List<Group>();
    private Donations _donations = new Donations();
    private string _additionalInformation = "additional information";
    private List<GroupBranding> _groupBranding = new List<GroupBranding>();
    private readonly List<Alert> _alerts = new List<Alert>();

    public ProcessedGroup Build()
    {
        return new ProcessedGroup
        {
            Name = _name,
            Slug = _slug,
            MetaDescription = _metaDescription,
            PhoneNumber = _phoneNumber,
            Email = _email,
            Website = _website,
            Twitter = _twitter,
            Facebook = _facebook,
            Address = _address,
            Description = _description,
            ImageUrl = _imageUrl,
            ThumbnailImageUrl = _thumbnailImageUrl,
            CategoriesReference = _categoriesReference,
            SubCategories = _subCategories,
            Breadcrumbs = _breadcrumbs,
            Events = _events,
            GroupAdministrators = _groupAdministrators,
            DateHiddenFrom = _dateHiddenFrom,
            DateHiddenTo = _dateHiddenTo,
            Cost = _cost,
            CostText = _costText,
            AbilityLevel = _abilityLevel,
            Favourite = _favourite,
            Volunteering = _volunteering,
            Organisation = _organisation,
            LinkedGroups = _linkedGroups,
            Donations = _donations,
            MapDetails = _mapDetails,
            AdditionalInformation = _additionalInformation,
            DonationsText = _donationsText,
            DonationsUrl = _donationsUrl,
            DateLastModified = _dateLastModified,
            GroupBranding = _groupBranding,
            Alerts = _alerts
        };
    }

    public ProcessedGroupBuilder Email(string value)
    {
        _email = value;
        return this;
    }

    public ProcessedGroupBuilder MetaDescription(string metaDescription)
    {
        _metaDescription = metaDescription;
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
