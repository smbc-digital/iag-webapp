using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using MimeKit.Cryptography;
using Moq;
using Org.BouncyCastle.Asn1.Mozilla;
using StockportWebapp.Config;
using StockportWebapp.Http;
using StockportWebapp.Validation;
using StockportWebappTests.Unit.Http;
using Xunit;

namespace StockportWebappTests.Unit.Validation
{
    public class ReCaptchaValidationTests
    {
        public Mock<IApplicationConfiguration> _config = new Mock<IApplicationConfiguration>();
        private Mock<IHttpClient> _httpClient;

        public ReCaptchaValidationTests()
        {
            _httpClient = new Mock<IHttpClient>();
            _config.Setup(x => x.GetReCaptchaKey()).Returns(AppSetting.GetAppSetting("recaptchakey"));
        }

        [Fact]
        public void ShouldReturnValidationSuccessWhenTokenIsCorrect()
        {
            //// Arrange
            //ValidateReCaptchaAttribute validation = new ValidateReCaptchaAttribute(_config.Object, _httpClient.Object);
            //ActionExecutingContext context = new ActionExecutingContext(null, null);

            //// Act
            //Task<> response = await validation.OnActionExecutionAsync()

            //// Assert
        }
    }
}
