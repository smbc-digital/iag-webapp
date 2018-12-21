using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Controllers;
using StockportWebapp.Models;
using Xunit;
using HttpResponse = StockportWebapp.Http.HttpResponse;
using Helper = StockportWebappTests_Unit.TestHelper;
using StockportWebapp.Config;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;
using Moq;
using System.Threading.Tasks;
using StockportWebapp.Repositories;

namespace StockportWebappTests_Unit.Unit.Controllers
{
    public class PaymentControllerTest
    {
        private readonly Mock<IProcessedContentRepository> _fakeRepository = new Mock<IProcessedContentRepository>();
        private readonly PaymentController _paymentController;
        private readonly Mock<IApplicationConfiguration> _applicationConfiguration = new Mock<IApplicationConfiguration>();
        private readonly Mock<IViewRender> _viewRender = new Mock<IViewRender>();

        public PaymentControllerTest()
        {
            _paymentController = new PaymentController(_fakeRepository.Object, _applicationConfiguration.Object, _viewRender.Object, null);
        }

        [Fact]
        public async Task ItReturnsAGroupWithProcessedBody()
        {
            var processedPayment = new ProcessedPayment(Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString,
                Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<Crumb>());

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
