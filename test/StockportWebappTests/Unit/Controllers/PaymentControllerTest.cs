using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Controllers;
using StockportWebapp.Models;
using Xunit;
using HttpResponse = StockportWebapp.Http.HttpResponse;
using StockportWebappTests.Unit.Fake;
using Helper = StockportWebappTests.TestHelper;

namespace StockportWebappTests.Unit.Controllers
{
    public class PaymentControllerTest
    {
        private readonly FakeProcessedContentRepository _fakeRepository;
        private readonly PaymentController _paymentController;

        public PaymentControllerTest()
        {
            _fakeRepository = new FakeProcessedContentRepository();
            _paymentController = new PaymentController(_fakeRepository);
        }

        [Fact]
        public void ItReturnsAGroupWithProcessedBody()
        {
            var processedPayment = new ProcessedPayment(Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString,
                Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<Crumb>());

            _fakeRepository.Set(new HttpResponse((int)HttpStatusCode.OK, processedPayment, string.Empty));

            var view = AsyncTestHelper.Resolve(_paymentController.Detail("slug")) as ViewResult;
            var model = view.ViewData.Model as ProcessedPayment;
            model.Should().Be(processedPayment);
        }

        [Fact]
        public void GetsA404NotFoundGroup()
        {
            _fakeRepository.Set(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

            var response = AsyncTestHelper.Resolve(_paymentController.Detail("not-found-slug")) as HttpResponse;

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
