using System;
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
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using StockportWebapp.Enums;
using StockportWebapp.Repositories;
using StockportWebappTests_Unit.Helpers;
using StockportGovUK.NetStandard.Gateways.Civica.Pay;
using Microsoft.Extensions.Configuration;
using StockportGovUK.NetStandard.Gateways.Response;
using StockportGovUK.NetStandard.Models.Civica.Pay.Request;
using StockportGovUK.NetStandard.Models.Civica.Pay.Response;

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
        public async Task DetailShouldReturnAPaymentWithProcessedBody()
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
        public async Task DetailShouldGetA404NotFoundPayment()
        {
            _fakeRepository
                .Setup(_ => _.Get<Payment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

            var response = await _paymentController.Detail("not-found-slug", null, null) as HttpResponse;;

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DetailPostShouldCallGatewayCreateImmediateBasket()
        {
            _civicaPayGateway.Setup(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()))
                .ReturnsAsync(new HttpResponse<CreateImmediateBasketResponse> { StatusCode = HttpStatusCode.OK, ResponseContent = new CreateImmediateBasketResponse { BasketReference = "testRef", BasketToken = "testBasketToken", ResponseCode = "00000"} });

            _civicaPayGateway
                .Setup(_ => _.GetPaymentUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("redirectUrl");

            var processedPayment = new ProcessedPayment(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString,
                TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Crumb>(),
                EPaymentReferenceValidation.None, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Alert>());

            _fakeRepository
                .Setup(_ => _.Get<Payment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedPayment, string.Empty));

            _configuration.Setup(_ => _.GetSection(It.IsAny<string>()))
                .Returns(new Mock<IConfigurationSection>().Object);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            _paymentController.ObjectValidator = objectValidator.Object;

            await _paymentController.Detail("slug", new PaymentSubmission
            {
                Payment = new ProcessedPayment("title", "slug", "teaser", "description", "payDetailsText", "refLabel", "fund", "glCode", null, EPaymentReferenceValidation.None, "meta", "returnUrl", "catId", "accRef", "payDesc", null),
                Amount = 12.00m,
                Reference = "123456789"
            });

            _civicaPayGateway.Verify(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Once);
        }

        [Fact]
        public async Task DetailPostShouldReturnDetailsViewIfModelStateInvalid()
        {
            var processedPayment = new ProcessedPayment(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString,
                TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Crumb>(),
                EPaymentReferenceValidation.None, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Alert>());

            _fakeRepository
                .Setup(_ => _.Get<Payment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedPayment, string.Empty));

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            _paymentController.ObjectValidator = objectValidator.Object;

            _paymentController.ModelState.AddModelError("Reference", "error");

            var result = await _paymentController.Detail("slug", new PaymentSubmission
            {
                Payment = new ProcessedPayment("title", "slug", "teaser", "description", "payDetailsText", "refLabel", "fund", "glCode", null, EPaymentReferenceValidation.None, "meta", "returnUrl", "catId", "accRef", "payDesc", null),
                Amount = 12.00m,
                Reference = "123456"
            }) as ViewResult;

            _civicaPayGateway.Verify(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Never);
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task DetailPostShouldReturnErrorViewIfBadRequest()
        {
            _civicaPayGateway.Setup(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()))
                .ReturnsAsync(new HttpResponse<CreateImmediateBasketResponse> { StatusCode = HttpStatusCode.BadRequest, ResponseContent = new CreateImmediateBasketResponse { BasketReference = "testRef", BasketToken = "testBasketToken", ResponseCode = "00000" } });

            var processedPayment = new ProcessedPayment(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString,
                TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Crumb>(),
                EPaymentReferenceValidation.None, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Alert>());

            _fakeRepository
                .Setup(_ => _.Get<Payment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedPayment, string.Empty));

            _configuration.Setup(_ => _.GetSection(It.IsAny<string>()))
                .Returns(new Mock<IConfigurationSection>().Object);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            _paymentController.ObjectValidator = objectValidator.Object;

            var result = await _paymentController.Detail("slug", new PaymentSubmission
            {
                Payment = new ProcessedPayment("title", "slug", "teaser", "description", "payDetailsText", "refLabel", "fund", "glCode", null, EPaymentReferenceValidation.None, "meta", "returnUrl", "catId", "accRef", "payDesc", null),
                Amount = 12.00m,
                Reference = "123456789"
            }) as ViewResult;

            _civicaPayGateway.Verify(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Once);
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().Be("Error");
        }

        [Fact]
        public async Task DetailPostShouldReturnViewIfResponseCodeIs00001()
        {
            _civicaPayGateway.Setup(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()))
                .ReturnsAsync(new HttpResponse<CreateImmediateBasketResponse> { StatusCode = HttpStatusCode.BadRequest, ResponseContent = new CreateImmediateBasketResponse { BasketReference = "testRef", BasketToken = "testBasketToken", ResponseCode = "00001" } });

            var processedPayment = new ProcessedPayment(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString,
                TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Crumb>(),
                EPaymentReferenceValidation.None, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Alert>());

            _fakeRepository
                .Setup(_ => _.Get<Payment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedPayment, string.Empty));

            _configuration.Setup(_ => _.GetSection(It.IsAny<string>()))
                .Returns(new Mock<IConfigurationSection>().Object);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            _paymentController.ObjectValidator = objectValidator.Object;

            var result = await _paymentController.Detail("slug", new PaymentSubmission
            {
                Payment = new ProcessedPayment("title", "slug", "teaser", "description", "payDetailsText", "refLabel", "fund", "glCode", null, EPaymentReferenceValidation.None, "meta", "returnUrl", "catId", "accRef", "payDesc", null),
                Amount = 12.00m,
                Reference = "123456789"
            }) as ViewResult;

            _civicaPayGateway.Verify(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Once);
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task DetailPostShouldCallGatewayGetPaymentUrl()
        {
            _civicaPayGateway.Setup(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()))
                .ReturnsAsync(new HttpResponse<CreateImmediateBasketResponse> { StatusCode = HttpStatusCode.OK, ResponseContent = new CreateImmediateBasketResponse { BasketReference = "testRef", BasketToken = "testBasketToken", ResponseCode = "00000" } });

            _civicaPayGateway
                .Setup(_ => _.GetPaymentUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("redirectUrl");

            var processedPayment = new ProcessedPayment(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString,
                TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Crumb>(),
                EPaymentReferenceValidation.None, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Alert>());

            _fakeRepository
                .Setup(_ => _.Get<Payment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedPayment, string.Empty));

            _configuration.Setup(_ => _.GetSection(It.IsAny<string>()))
                .Returns(new Mock<IConfigurationSection>().Object);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            _paymentController.ObjectValidator = objectValidator.Object;

            await _paymentController.Detail("slug", new PaymentSubmission
            {
                Payment = new ProcessedPayment("title", "slug", "teaser", "description", "payDetailsText", "refLabel", "fund", "glCode", null, EPaymentReferenceValidation.None, "meta", "returnUrl", "catId", "accRef", "payDesc", null),
                Amount = 12.00m,
                Reference = "123456789"
            });

            _civicaPayGateway.Verify(_ => _.GetPaymentUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
