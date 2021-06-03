using StockportWebapp.Extensions;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Extensions
{
    public class SocialLinksExtensionTest
    {
        SocialLinksExtension socialLinksExtension = new SocialLinksExtension();

        [Fact]
        public void ShouldReturnFacebookDisplayUrlFromFullUrl()
        {
            var url = "http://www.facebook.com/zumba";
            var result = socialLinksExtension.GetSubstring(url);           

            Assert.Equal("/zumba", result);
        }

        [Fact]
        public void ShouldReturnTwitterDisplayUrlFromFullUrl()
        {
            var url = "http://www.twitter.com/zumba";
            var result = socialLinksExtension.GetSubstring(url);

            Assert.Equal("@zumba", result);
        }
    }
}
