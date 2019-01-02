using System.Net;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Controllers;
using Xunit;
using FluentAssertions;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.FeatureToggling;
using System.Threading.Tasks;

namespace StockportWebappTests_Unit.Unit.Controllers
{
    public class SearchControllerTest
    {
        private readonly SearchController _searchController;
        private readonly Mock<IApplicationConfiguration> _config;
        private string _businessId = "businessId";
        private const string SearchUrl = "search-url=";
        private const string PostcodeUrl = "postcode_url";

        public SearchControllerTest()
        {
            _config = new Mock<IApplicationConfiguration>();

            _searchController = new SearchController(_config.Object, new BusinessId(_businessId));
        }

        [Fact]
        public async Task ItRedirectsToPostcodeSearchUrl()
        {
            var postcodeSearchSetting = AppSetting.GetAppSetting(PostcodeUrl);
            _config.Setup(o => o.GetPostcodeSearchUrl(_businessId)).Returns(postcodeSearchSetting);

            var postcode = "m45 3fz";
            var result = await _searchController.Postcode(postcode);

            result.Should().BeOfType<RedirectResult>();
            
            _config.Verify(o => o.GetPostcodeSearchUrl(_businessId), Times.Once);
            var redirect = result as RedirectResult;
            redirect.Url.Should().Be(PostcodeUrl + postcode);
        }

        [Fact]
        public async Task ShouldRedierctToApplicationErrorIfPostCodeUrlConfigurationIsMissing()
        {
            var postCodeSearchSetting = AppSetting.GetAppSetting(null);
            _config.Setup(o => o.GetPostcodeSearchUrl(_businessId)).Returns(postCodeSearchSetting);

            var result = await _searchController.Postcode("a-postcode") as StatusCodeResult;

            result.StatusCode.Should().Be((int) HttpStatusCode.NotFound);            
        }
    }
}