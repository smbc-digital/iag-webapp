using System;
using System.Collections.Generic;
using Moq;
using StockportWebapp.ContentFactory;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;
using Xunit;
using FluentAssertions;
using Org.BouncyCastle.Security;


namespace StockportWebappTests.Unit.ContentFactory
{
    public class GroupFactoryTest
    {
        private readonly GroupFactory _factory;
        private readonly Mock<MarkdownWrapper> _markdownWrapper;
        private readonly Mock<ISimpleTagParserContainer> _tagParserContainer;
        private readonly Mock<IDynamicTagParser<Document>> _documentTagParser;
        private readonly Group _group;
        private const string Name = "Friends of Stockport";
        private const string Description = "Description";
        private const string Slug = "group";
        private const string Address = "Bramall Hall, Carpark, SK7 6HG";
        private const string Website = "http://www.fos.org.uk";
        private const string Email = "email";
        private const string PhoneNumber = "phonenumber";
        private const string Image = "image.jpg";
        private const string ThumbnailImage = "thumbnail.jpg";
        private const string Facebook = "facebook";
        private const string Twitter = "twitter";

        private const string Cost = "free";
        private const string CostText = "cost";
        private const string AbilityLevel = "level";

        private readonly List<Crumb> _breadcrumbs = new List<Crumb>();
        private List<GroupCategory> CategoriesReference = new List<GroupCategory>();
        private MapPosition _mapPosition = new MapPosition() {Lat=39.0, Lon = 2.0};
        private bool _volunteering = false;
        private List<Event> Events = new List<Event>();
        private GroupAdministrators _groupAdministrators = new GroupAdministrators();


        public GroupFactoryTest()
        {
            _markdownWrapper = new Mock<MarkdownWrapper>();
            _tagParserContainer = new Mock<ISimpleTagParserContainer>();
            _documentTagParser = new Mock<IDynamicTagParser<Document>>();
            _factory = new GroupFactory(_tagParserContainer.Object, _markdownWrapper.Object);
            _group = new Group(
                name: Name,
                slug: Slug,
                description: Description,
                imageUrl: Image,
                thumbnailImageUrl: ThumbnailImage,
                address: Address,
                website: Website,
                email: Email,
                phoneNumber: PhoneNumber,
                breadcrumbs: _breadcrumbs,
                categoriesReference: CategoriesReference,
                subCategories: null,
                facebook : Facebook,
                twitter : Twitter,
                mapPosition:_mapPosition,
                volunteering:_volunteering,
                events: Events,
                groupAdministrators: _groupAdministrators,
                dateHiddenFrom: DateTime.MinValue,
                dateHiddenTo: DateTime.MinValue,
                status: "published",
                cost: Cost,
                costText: CostText,
                abilityLevel: AbilityLevel,
                favourite: false,
                volunteeringText: "text",
                organisation: null,
                donations: false
            );

            _tagParserContainer.Setup(o => o.ParseAll(Description, It.IsAny<string>())).Returns(Description);
            _markdownWrapper.Setup(o => o.ConvertToHtml(Description)).Returns(Description);            
        }

        [Fact]
        public void ShouldSetTheCorrespondingFieldsForAProcessedGroup()
        {
            var result = _factory.Build(_group);
            result.Name.Should().Be("Friends of Stockport");
            result.Description.Should().Be("Description");
            result.Slug.Should().Be("group");
            result.Address.Should().Be("Bramall Hall, Carpark, SK7 6HG");
            result.Website.Should().Be("http://www.fos.org.uk");
            result.Email.Should().Be("email");
            result.PhoneNumber.Should().Be("phonenumber");
            result.ImageUrl.Should().Be("image.jpg");
            result.MapPosition.Lat.Should().Be(39.0);
            result.MapPosition.Lon.Should().Be(2.0);
            result.ThumbnailImageUrl.Should().Be("thumbnail.jpg");
            result.Twitter.Should().Be("twitter");
            result.Facebook.Should().Be("facebook");
            result.Volunteering.VolunteeringNeeded.Should().Be(false);
            result.Volunteering.VolunteeringText.Should().Be("text");

        }

        [Fact]
        public void ShouldProcessDescriptionWithMarkdown()
        {
            _factory.Build(_group);

            _markdownWrapper.Verify(o => o.ConvertToHtml(Description), Times.Once);
        }

        [Fact]
        public void ShouldPassTitleToAllSimpleParsersWhenBuilding()
        {
            _factory.Build(_group);

            _tagParserContainer.Verify(o => o.ParseAll(Description, _group.Name), Times.Once);
        }      
    }
}
