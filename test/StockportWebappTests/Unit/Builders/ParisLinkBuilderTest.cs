using Xunit;
using FluentAssertions;
using StockportWebapp.Config;
using Moq;

namespace StockportWebappTests.Unit.Builders
{
    public class ParisLinkBuilderTest
    {
        private readonly IParisLinkBuilder _parisLinkBuilder;
        ParisRecordXML parisRecordXML;
        string parisRecordXMLStringOutput;
        private readonly Mock<IApplicationConfiguration> _config;
        private const string BusinessId = "businessId";

        public ParisLinkBuilderTest()
        {
            _parisLinkBuilder = new ParisLinkBuilder();
            parisRecordXML = new ParisRecordXML() { amount = "amount", fund = "fund", reference = "reference", text6 = "text6" };
            parisRecordXMLStringOutput = "<records><record><reference>reference</reference><fund>fund</fund><amount>amount</amount><text6>text6</text6></record></records>";

            _config = new Mock<IApplicationConfiguration>();
            _config.Setup(o => o.GetParisPamentLink(BusinessId)).Returns(AppSetting.GetAppSetting("ParisPayment"));
        }

        [Fact]
        public void ShouldReturnXMLWhenReferencesObjectProvided()
        {
            var parisLink = _parisLinkBuilder.ParisRecordXML(parisRecordXML).Build(_config.Object);

            parisLink.Should().NotBeNull();

            parisLink.Should().Contain(parisRecordXMLStringOutput);
        }

        [Fact]
        public void ShouldReturnEmptyValuesWhenNoneAreProvided()
        {
            var parisLink = _parisLinkBuilder.Build(_config.Object);

            parisLink.Should().EndWith("?returntext=&ignoreconfirmation=&payforbasketmode=&data=&recordxml=&returnurl=");
        }

        [Fact]
        public void ShouldReturnFullQueryStringWhenAllValuesAreSet()
        {
            var parisLink = _parisLinkBuilder.PayForBasketMode("True")
                                             .IgnoreConfirmation("True")
                                             .Data("Data")
                                             .ParisRecordXML(parisRecordXML)
                                             .ReturnText("Test")
                                             .ReturnUrl("Test")
                                             .Build(_config.Object);

            parisLink.Should().EndWith("?returntext=Test&ignoreconfirmation=True&payforbasketmode=True&data=Data&recordxml=" + parisRecordXMLStringOutput + "&returnurl=Test");
        }
    }
}
