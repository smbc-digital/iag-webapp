using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Controllers;
using StockportWebapp.Models;
using Xunit;
using HttpResponse = StockportWebapp.Http.HttpResponse;
using StockportWebapp.ProcessedModels;
using Moq;
using System.Threading.Tasks;
using StockportWebapp.Enums;
using StockportWebapp.Repositories;
using StockportWebappTests_Unit.Helpers;
using StockportGovUK.NetStandard.Gateways.Civica.Pay;
using Microsoft.Extensions.Configuration;

namespace StockportWebappTests_Unit.Unit.Controllers
{
    public class PaymentControllerTest
    {
        private readonly Mock<IProcessedContentRepository> _fakeRepository = new Mock<IProcessedContentRepository>();
        private readonly PaymentController _paymentController;
        private readonly Mock<ICivicaPayGateway> _civicaPayGateway = new Mock<ICivicaPayGateway>();
        private readonly Mock<IConfiguration> _configuration = new Mock<IConfiguration>();

        public PaymentControllerTest()
        {
            _paymentController = new PaymentController(_fakeRepository.Object, _civicaPayGateway.Object, _configuration.Object);
        }

        [Fact]
        public async Task ItReturnsAGroupWithProcessedBody()
        {
            var processedPayment = new ProcessedPayment(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString,
                TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Crumb>(),
                EPaymentReferenceValidation.None, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Alert>());

            _fakeRepository
                .Setup(_ => _.Get<Payment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedPayment, string.Empty));

            var view = await _paymentController.Detail("slug", null, null) as ViewResult;;
            var model = view.ViewData.Model as PaymentSubmission;
            model.Payment.Should().Be(processedPayment);
        }

        [Fact]
        public async Task GetsA404NotFoundGroup()
        {
            _fakeRepository
                .Setup(_ => _.Get<Payment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

            var response = await _paymentController.Detail("not-found-slug", null, null) as HttpResponse;;

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
