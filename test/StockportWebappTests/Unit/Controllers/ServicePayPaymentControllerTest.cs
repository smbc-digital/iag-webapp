using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Configuration;
using Moq;
using StockportGovUK.NetStandard.Gateways.Civica.Pay;
using StockportGovUK.NetStandard.Gateways.Response;
using StockportGovUK.NetStandard.Models.Civica.Pay.Request;
using StockportGovUK.NetStandard.Models.Civica.Pay.Response;
using StockportWebapp.Controllers;
using StockportWebapp.Enums;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Repositories;
using Xunit;
using HttpResponse = StockportWebapp.Http.HttpResponse;

namespace StockportWebappTests_Unit.Unit.Controllers
{
    public class ServicePayPaymentControllerTest
    {
        private readonly Mock<IProcessedContentRepository> _fakeRepository = new Mock<IProcessedContentRepository>();
        private readonly ServicePayPaymentController _paymentController;
        private readonly Mock<ICivicaPayGateway> _civicaPayGateway = new Mock<ICivicaPayGateway>();
        private readonly Mock<IConfiguration> _configuration = new Mock<IConfiguration>();

        public ServicePayPaymentControllerTest()
        {
            _paymentController = new ServicePayPaymentController(_fakeRepository.Object, _civicaPayGateway.Object, _configuration.Object);
        }

        [Fact]
        public async Task DetailShouldReturnAPaymentWithProcessedBody()
        {
            var processedPayment = new ProcessedServicePayPayment(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), new List<Crumb>(), EPaymentReferenceValidation.None,
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<Alert>(), It.IsAny<string>());

            _fakeRepository
                .Setup(_ => _.Get<ServicePayPayment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedPayment, string.Empty));

            var view = await _paymentController.Detail("slug", null, null) as ViewResult;;
            var model = view.ViewData.Model as ServicePayPaymentSubmission;
            model.Payment.Should().Be(processedPayment);
        }

        [Fact]
        public async Task DetailShouldGetA404NotFoundPayment()
        {
            _fakeRepository
                .Setup(_ => _.Get<ServicePayPayment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
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

            var processedPayment = new ProcessedServicePayPayment("title", "slug", "teaser", "description", "paymentDetailsText",
                "Reference Number", new List<Crumb>(), EPaymentReferenceValidation.None,
                "metaDescription", "returnUrl", "1233455", "40000000", "paymentDescription", new List<Alert>(), "20.65");

            _fakeRepository
                .Setup(_ => _.Get<ServicePayPayment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedPayment, string.Empty));

            _configuration
                .Setup(_ => _.GetSection(It.IsAny<string>()))
                .Returns(new Mock<IConfigurationSection>().Object);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(_ => _.Validate(It.IsAny<ActionContext>(), It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(), It.IsAny<object>()));
            _paymentController.ObjectValidator = objectValidator.Object;

            await _paymentController.Detail("slug", new ServicePayPaymentSubmission
            {
                Reference = "12346",
                Amount = 23.5m,
                Name = "name",
                EmailAddress = "test-email-address",
                Payment = processedPayment
            });

            _civicaPayGateway.Verify(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Once);
        }

        [Fact]
        public async Task DetailPostShouldReturnDetailsViewIfModelStateInvalid()
        {
            var processedPayment = new ProcessedServicePayPayment("title", "slug", "teaser", "description", "paymentDetailsText",
                "Reference Number", new List<Crumb>(), EPaymentReferenceValidation.None,
                "metaDescription", "returnUrl", "1233455", "40000000", "paymentDescription", new List<Alert>(), "20.65");

            _fakeRepository
                .Setup(_ => _.Get<ServicePayPayment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedPayment, string.Empty));

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<object>()));
            _paymentController.ObjectValidator = objectValidator.Object;

            _paymentController.ModelState.AddModelError("Reference", "error");

            var result = await _paymentController.Detail("slug", new ServicePayPaymentSubmission
            {
                Payment = processedPayment,
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

            var processedPayment = new ProcessedServicePayPayment("title", "slug", "teaser", "description", "paymentDetailsText",
                "Reference Number", new List<Crumb>(), EPaymentReferenceValidation.None,
                "metaDescription", "returnUrl", "1233455", "40000000", "paymentDescription", new List<Alert>(), "20.65");

            _fakeRepository
                .Setup(_ => _.Get<ServicePayPayment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedPayment, string.Empty));

            _configuration.Setup(_ => _.GetSection(It.IsAny<string>()))
                .Returns(new Mock<IConfigurationSection>().Object);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<object>()));
            _paymentController.ObjectValidator = objectValidator.Object;

            var result = await _paymentController.Detail("slug", new ServicePayPaymentSubmission
            {
                Payment = processedPayment,
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

            var processedPayment = new ProcessedServicePayPayment("title", "slug", "teaser", "description", "paymentDetailsText",
                "Reference Number", new List<Crumb>(), EPaymentReferenceValidation.None,
                "metaDescription", "returnUrl", "1233455", "40000000", "paymentDescription", new List<Alert>(), "20.65");

            _fakeRepository
                .Setup(_ => _.Get<ServicePayPayment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedPayment, string.Empty));

            _configuration.Setup(_ => _.GetSection(It.IsAny<string>()))
                .Returns(new Mock<IConfigurationSection>().Object);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<object>()));
            _paymentController.ObjectValidator = objectValidator.Object;

            var result = await _paymentController.Detail("slug", new ServicePayPaymentSubmission
            {
                Payment = processedPayment,
                Reference = "123456789",
                Amount = 12.00m,
                EmailAddress = "test-email-address"
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

            var processedPayment = new ProcessedServicePayPayment("title", "slug", "teaser", "description", "paymentDetailsText",
                "Reference Number", new List<Crumb>(), EPaymentReferenceValidation.None,
                "metaDescription", "returnUrl", "1233455", "40000000", "paymentDescription", new List<Alert>(), "20.65");

            _fakeRepository
                .Setup(_ => _.Get<ServicePayPayment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedPayment, string.Empty));

            _configuration.Setup(_ => _.GetSection(It.IsAny<string>()))
                .Returns(new Mock<IConfigurationSection>().Object);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<object>()));
            _paymentController.ObjectValidator = objectValidator.Object;

            await _paymentController.Detail("slug", new ServicePayPaymentSubmission
            {
                Payment = processedPayment,
                Amount = 12.00m,
                Reference = "123456789"
            });

            _civicaPayGateway.Verify(_ => _.GetPaymentUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
