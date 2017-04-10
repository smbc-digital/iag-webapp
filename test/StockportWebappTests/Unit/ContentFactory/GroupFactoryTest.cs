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
        private const string Description = "The event";
        private const string Slug = "group";
        private const string Address = "Bramall Hall, Carpark, SK7 6HG";
        private const string Website = "http://www.fos.org.uk";
        private const string Email = "email";
        private const string PhoneNumber = "phonenumber";
        private const string Image = "image.jpg";
        private const string ThumbnailImage = "thumbnail.jpg";
        private const string Facebook = "facebook";
        private const string Twitter = "twitter";
        private readonly List<Crumb> _breadcrumbs = new List<Crumb>();
        private List<GroupCategory> CategoriesReference = new List<GroupCategory>();


        public GroupFactoryTest()
        {
            _markdownWrapper = new Mock<MarkdownWrapper>();
            _tagParserContainer = new Mock<ISimpleTagParserContainer>();
            _documentTagParser = new Mock<IDynamicTagParser<Document>>();
            _factory = new GroupFactory(_tagParserContainer.Object, _markdownWrapper.Object);
            _group = new Group(            
                name : Name,
                slug : Slug,
                description : Description,
                imageUrl : Image,
                thumbnailImageUrl : ThumbnailImage,
                address : Address,
                website : Website,
                email :Email,
                phoneNumber : PhoneNumber,
                breadcrumbs : _breadcrumbs,
                categoriesReference : CategoriesReference,
                facebook : Facebook,
                twitter : Twitter
            );

            _tagParserContainer.Setup(o => o.ParseAll(Description, It.IsAny<string>())).Returns(Description);
            _markdownWrapper.Setup(o => o.ConvertToHtml(Description)).Returns(Description);            
        }

        [Fact]
        public void ShouldSetTheCorrespondingFieldsForAProcessedEvent()
        {
            var result = _factory.Build(_group);
            result.Name.Should().Be("Friends of Stockport");

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
