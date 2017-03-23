using FluentAssertions;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Helpers;
using StockportWebapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StockportWebappTests.Unit.Helpers
{
    public class ParisLinkHelperTest
    {
        private readonly Mock<IApplicationConfiguration> _config;
        private const string BusinessId = "businessId";
        private ProcessedPayment testProcessedPayment = new ProcessedPayment("title",
                                                                     "slug",
                                                                     "teaser",
                                                                     "description",
                                                                     "paymentDetailsText",
                                                                     "referenceLabel",
                                                                     "parisReference",
                                                                     "07",
                                                                     "glCodeCostCentreNumber",
                                                                     new List<Crumb>());
        public ParisLinkHelperTest()
        {
            _config = new Mock<IApplicationConfiguration>();
            _config.Setup(o => o.GetParisPamentLink(BusinessId)).Returns(AppSetting.GetAppSetting("ParisPayment"));
        }

        [Fact]
        public void ShouldReturnFund15WhenFundIs07AndLengthIs10()
        {
            PaymentSubmission paymentSubmission = new PaymentSubmission() { Reference = "1234567890", Payment = testProcessedPayment };

            string parisLink = ParisLinkHelper.CreateParisLink(paymentSubmission, _config.Object);

            parisLink.Should().Contain("<fund>15</fund>");
        }

        [Fact]
        public void ReferenceShouldBeGlCodeNumberAndText6ShouldBeUserInputIfGlCodeNumberIsSupplied()
        {
            PaymentSubmission paymentSubmission = new PaymentSubmission() { Reference = "test", Payment = testProcessedPayment };

            string parisLink = ParisLinkHelper.CreateParisLink(paymentSubmission, _config.Object);

            parisLink.Should().Contain("<reference>glCodeCostCentreNumber</reference>");
            parisLink.Should().Contain("<text6>test</text6>");
        }

        [Fact]
        public void ReferenceShouldBeUserInputAndText6ShouldBeTitleIfGlCodeNumberIsNotSupplied()
        {
            ProcessedPayment testProcessedPaymentWithoutglCodeCostCentreNumber = new ProcessedPayment("title",
                                                                                                      "slug",
                                                                                                      "teaser",
                                                                                                      "description",
                                                                                                      "paymentDetailsText",
                                                                                                      "referenceLabel",
                                                                                                      "parisReference",
                                                                                                      "07",
                                                                                                      "",
                                                                                                      new List<Crumb>());
            PaymentSubmission paymentSubmission = new PaymentSubmission() { Reference = "test", Payment = testProcessedPaymentWithoutglCodeCostCentreNumber };

            string parisLink = ParisLinkHelper.CreateParisLink(paymentSubmission, _config.Object);

            parisLink.Should().Contain("<reference>test</reference>");
            parisLink.Should().Contain("<text6>title</text6>");
        }
    }
}
