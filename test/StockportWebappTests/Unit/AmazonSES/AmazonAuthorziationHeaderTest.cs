using FluentAssertions;
using StockportWebapp.AmazonSES;
using StockportWebapp.Config;
using Xunit;

namespace StockportWebappTests.Unit.AmazonSES
{
    public class AmazonAuthorziationHeaderTest
    {
        [Fact]
        public void ItComputesAmazonAuthorizationHeaderForAmazonSesService()
        {
            var amazonSesConfig = new AmazonSesClientConfiguration(AppSetting.GetAppSetting("fake.host"), AppSetting.GetAppSetting("region"),
                AppSetting.GetAppSetting(TestHelper.AnyString), new AmazonSESKeys("account-id", "access-key"));

            const string dateStamp = "20160718";
            const string amzDate = "20160718T142726Z";
            var payload =
                string.Concat("Action=SendEmail&Destination.ToAddresses.member.1=destination@email.com&",
                "Message.Subject.Data=Message%20Subject&Message.Body.Text.Data=Message%20body&",
                "Source=source@email.com");

            var amazonAuthHeader = new AmazonAuthorizationHeader();

            var expectedHeader =
                string.Concat("AWS4-HMAC-SHA256 Credential=account-id/20160718/region/email/aws4_request, ",
                "SignedHeaders=content-type;host;x-amz-date, ",
                "Signature=3ae99c154110d2257a759380ec1ec3f2f1d9db4acf1d3fa740161fb9be7022dd");

            var authorizationHeader = amazonAuthHeader.Create(amazonSesConfig, payload, dateStamp, amzDate);

            authorizationHeader.Should().Be(expectedHeader);
        }

        [Fact]
        public void ItShouldDeriveTheSigningKey()
        {
            var authHeader = new AmazonAuthorizationHeader();

            var signingKey = authHeader.CalculateSignatureKey("access-key", "20160718", "region");

            TestHelper.ByteArrayToHexaString(signingKey).Should().Be("b255e8cd33388d746076499884df1d5c31600a06dd9323fe54d7fa7dfd473073");
        }
    }
}