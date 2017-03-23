using StockportWebapp.Config;
using StockportWebapp.Models;
using StockportWebappTests.Unit.Builders;

namespace StockportWebapp.Helpers
{
    public static class ParisLinkHelper
    {
        private static IParisLinkBuilder _parisLinkBuilder;
        public static string CreateParisLink(PaymentSubmission paymentSubmission, IApplicationConfiguration _applicationConfiguration)
        {
            _parisLinkBuilder = new ParisLinkBuilder();

            var processedReference = paymentSubmission.Reference;
            var processedText6 = paymentSubmission.Payment.Title;
            if (!string.IsNullOrEmpty(paymentSubmission.Payment.GlCodeCostCentreNumber))
            {
                processedReference = paymentSubmission.Payment.GlCodeCostCentreNumber;
                processedText6 = paymentSubmission.Reference;
            }

            string processedFund = paymentSubmission.Payment.Fund;
            if (paymentSubmission.Payment.Fund == "07" && paymentSubmission.Reference.Length == 10)
                processedFund = "15";

            ParisRecordXML xml = new ParisRecordXML()
            {
                amount = paymentSubmission.Amount.ToString(),
                fund = processedFund,
                reference = processedReference,
                text6 = processedText6,
                memo = paymentSubmission.Reference
            };

            return _parisLinkBuilder.ReturnText("Return To Main Menu")
                             .IgnoreConfirmation("false")
                             .PayForBasketMode("true")
                             .Data(paymentSubmission.Payment.ParisReference)
                             .ParisRecordXML(xml)
                             //.ReturnUrl(Request.GetUri().AbsoluteUri)
                             .ReturnUrl("https://www.stockport.gov.uk")
                             .Build(_applicationConfiguration);
        }
    }
}
