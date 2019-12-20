using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Controllers;
using StockportWebapp.Models;
using Xunit;
using HttpResponse = StockportWebapp.Http.HttpResponse;
using StockportWebapp.Config;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;
using Moq;
using System.Threading.Tasks;
using StockportWebapp.Enums;
using StockportWebapp.Repositories;
using StockportWebappTests_Unit.Helpers;
using StockportGovUK.NetStandard.Gateways.Civica.Pay;
using StockportWebapp.FeatureToggling;
using Microsoft.Extensions.Configuration;

namespace StockportWebappTests_Unit.Unit.Controllers
{
    public class PaymentControllerTest
    {
        private readonly Mock<IProcessedContentRepository> _fakeRepository = new Mock<IProcessedContentRepository>();
        private readonly PaymentController _paymentController;
        private readonly Mock<IViewRender> _viewRender = new Mock<IViewRender>();
        private readonly Mock<ICivicaPayGateway> _civicaPayGateway = new Mock<ICivicaPayGateway>();
        private readonly Mock<FeatureToggles> _featureToggles = new Mock<FeatureToggles>();
        private readonly Mock<IConfiguration> _configuration = new Mock<IConfiguration>();
        private readonly Mock<IApplicationConfiguration> _applicationConfiguration = new Mock<IApplicationConfiguration>();

        public PaymentControllerTest()
        {
            _paymentController = new PaymentController(_fakeRepository.Object,_viewRender.Object, null, _civicaPayGateway.Object, _configuration.Object, _featureToggles.Object, _applicationConfiguration.Object);
        }

        [Fact]
        public async Task ItReturnsAGroupWithProcessedBody()
        {
            var processedPayment = new ProcessedPayment(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString,
                TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Crumb>(),
                EPaymentReferenceValidation.None, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString);

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
