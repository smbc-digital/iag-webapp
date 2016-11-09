using System.Net;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Controllers;
using Xunit;
using FluentAssertions;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Models;

namespace StockportWebappTests.Unit.Controllers
{
    public class SearchControllerTest
    {
        private readonly SearchController _searchController;
        private Mock<IApplicationConfiguration> _config;
        private string _businessId = "businessId";
        private const string SearchUrl = "search-url=";
        private const string PostcodeUrl = "postcode_url";

        public SearchControllerTest()
        {
            _config = new Mock<IApplicationConfiguration>();

            _searchController = new SearchController(_config.Object, new BusinessId(_businessId));
        }

        [Fact]
        public void ItRedirectsToSearchUrl()
        {
            var searchUrlSetting = AppSetting.GetAppSetting(SearchUrl);
            _config.Setup(o => o.GetSearchUrl(_businessId)).Returns(searchUrlSetting);

            var searchTerm = "Cars";
            var result = AsyncTestHelper.Resolve(_searchController.Index(searchTerm));

            result.Should().BeOfType<RedirectResult>();

            _config.Verify(o => o.GetSearchUrl(_businessId), Times.Once);
            var redirect = result as RedirectResult;
            redirect.Url.Should().Be(SearchUrl + searchTerm);
        }

        [Fact]
        public void ShouldRedirectToApplicationErrorIfSearchConfigurationIsMissing()
        {
            const string searchTerm = "term";
            var appSetting = AppSetting.GetAppSetting(null);
            _config.Setup(o => o.GetSearchUrl(_businessId)).Returns(appSetting);

            var response = AsyncTestHelper.Resolve(_searchController.Index(searchTerm)) as StatusCodeResult;

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public void ItRedirectsToPostcodeSearchUrl()
        {
            var postcodeSearchSetting = AppSetting.GetAppSetting(PostcodeUrl);
            _config.Setup(o => o.GetPostcodeSearchUrl(_businessId)).Returns(postcodeSearchSetting);

            var postcode = "m45 3fz";
            var result = AsyncTestHelper.Resolve(_searchController.Postcode(postcode));

            result.Should().BeOfType<RedirectResult>();
            
            _config.Verify(o => o.GetPostcodeSearchUrl(_businessId), Times.Once);
            var redirect = result as RedirectResult;
            redirect.Url.Should().Be(PostcodeUrl + postcode);
        }

        [Fact]
        public void ShouldRedierctToApplicationErrorIfPostCodeUrlConfigurationIsMissing()
        {
            var postCodeSearchSetting = AppSetting.GetAppSetting(null);
            _config.Setup(o => o.GetPostcodeSearchUrl(_businessId)).Returns(postCodeSearchSetting);

            var result = AsyncTestHelper.Resolve(_searchController.Postcode("a-postcode")) as StatusCodeResult;

            result.StatusCode.Should().Be((int) HttpStatusCode.NotFound);            
        }
    }
}