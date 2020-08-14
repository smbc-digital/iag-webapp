using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
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
    public class PaymentControllerTest
    {
        private readonly Mock<IProcessedContentRepository> _fakeRepository = new Mock<IProcessedContentRepository>();
        private readonly PaymentController _paymentController;
        private readonly PaymentController _paymentControllerWithServicePayPaymentPath;
        private readonly Mock<ICivicaPayGateway> _civicaPayGateway = new Mock<ICivicaPayGateway>();
        private readonly Mock<IConfiguration> _configuration = new Mock<IConfiguration>();
        private readonly Mock<IObjectModelValidator> _objectValidator = new Mock<IObjectModelValidator>();

        private readonly ProcessedPayment _processedPayment = new ProcessedPayment(
            "title", 
            "slug", 
            "teaser", 
            "description", 
            "payDetailsText", 
            "refLabel", 
            "fund", 
            "glCode", 
            null, 
            EPaymentReferenceValidation.None, 
            "meta", 
            "returnUrl", 
            "catId", 
            "accRef", 
            "payDesc", 
            null);

        private readonly ProcessedServicePayPayment _processedServicePayPayment = new ProcessedServicePayPayment(
            "title",
            "slug",
            "teaser",
            "description",
            "payDetailsText",
            "refLabel",
            null,
            EPaymentReferenceValidation.None,
            "meta",
            "returnUrl",
            "catId",
            "accRef",
            "payDesc",
            null,
            "payAmount");

        public PaymentControllerTest()
        {
            var httpContextPayment = new DefaultHttpContext();
            httpContextPayment.Request.Path = "/payment";
            var mockControllerContextPayment = new ControllerContext
            {
                HttpContext = httpContextPayment
            };

            var httpContextServicePayPayment = new DefaultHttpContext();
            httpContextServicePayPayment.Request.Path = "/service-pay-payment";
            var mockControllerContextServicePayPayment = new ControllerContext
            {
                HttpContext = httpContextServicePayPayment
            };

            _fakeRepository
                .Setup(_ => _.Get<Payment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, _processedPayment, string.Empty));

            _fakeRepository
                .Setup(_ => _.Get<ServicePayPayment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int) HttpStatusCode.OK, _processedServicePayPayment, string.Empty));

            _configuration
                .Setup(_ => _.GetSection(It.IsAny<string>()))
                .Returns(new Mock<IConfigurationSection>().Object);

            _objectValidator
                .Setup(o => o.Validate(
                    It.IsAny<ActionContext>(),
                    It.IsAny<ValidationStateDictionary>(),
                    It.IsAny<string>(),
                    It.IsAny<Object>()));

            _paymentController = new PaymentController(_fakeRepository.Object, _civicaPayGateway.Object, _configuration.Object)
            {
                ControllerContext = mockControllerContextPayment
            };

            _paymentControllerWithServicePayPaymentPath = new PaymentController(_fakeRepository.Object, _civicaPayGateway.Object, _configuration.Object)
            {
                ControllerContext = mockControllerContextServicePayPayment
            };
        }

        [Fact]
        public async Task DetailShouldReturnAPaymentWithProcessedBody()
        {
            var view = await _paymentController.Detail("slug", null, null) as ViewResult;;
            var model = view.ViewData.Model as PaymentSubmission;
            model.Payment.Should().Be(_processedPayment);
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
            _civicaPayGateway
                .Setup(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()))
                .ReturnsAsync(new HttpResponse<CreateImmediateBasketResponse>
                {
                    StatusCode = HttpStatusCode.OK, 
                    ResponseContent = new CreateImmediateBasketResponse
                    {
                        BasketReference = "testRef", 
                        BasketToken = "testBasketToken", 
                        ResponseCode = "00000"
                    }
                });

            _civicaPayGateway
                .Setup(_ => _.GetPaymentUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("redirectUrl");

            _paymentController.ObjectValidator = _objectValidator.Object;

            await _paymentController.Detail("slug", new PaymentSubmission
            {
                Payment = _processedPayment,
                Amount = 12.00m,
                Reference = "123456789"
            });

            _civicaPayGateway.Verify(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Once);
        }

        [Fact]
        public async Task DetailPostShouldReturnDetailsViewIfModelStateInvalid()
        {
            _paymentController.ObjectValidator = _objectValidator.Object;

            _paymentController.ModelState.AddModelError("Reference", "error");

            var result = await _paymentController.Detail("slug", new PaymentSubmission
            {
                Payment = _processedPayment,
                Amount = 12.00m,
                Reference = "123456"
            }) as ViewResult;

            _civicaPayGateway.Verify(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Never);
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task DetailPostShouldReturnErrorViewIfBadRequest()
        {
            _civicaPayGateway
                .Setup(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()))
                .ReturnsAsync(new HttpResponse<CreateImmediateBasketResponse>
                {
                    StatusCode = HttpStatusCode.BadRequest, 
                    ResponseContent = new CreateImmediateBasketResponse
                    {
                        BasketReference = "testRef", 
                        BasketToken = "testBasketToken", 
                        ResponseCode = "00000"
                    }
                });

            _paymentController.ObjectValidator = _objectValidator.Object;

            var result = await _paymentController.Detail("slug", new PaymentSubmission
            {
                Payment = _processedPayment,
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
            _civicaPayGateway
                .Setup(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()))
                .ReturnsAsync(new HttpResponse<CreateImmediateBasketResponse>
                {
                    StatusCode = HttpStatusCode.BadRequest, 
                    ResponseContent = new CreateImmediateBasketResponse
                    {
                        BasketReference = "testRef", 
                        BasketToken = "testBasketToken", 
                        ResponseCode = "00001"
                    }
                });

            _paymentController.ObjectValidator = _objectValidator.Object;

            var result = await _paymentController.Detail("slug", new PaymentSubmission
            {
                Payment = _processedPayment,
                Amount = 12.00m,
                Reference = "123456789"
            }) as ViewResult;

            _civicaPayGateway.Verify(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Once);
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task DetailPostShouldCallGatewayGetPaymentUrl()
        {
            _civicaPayGateway
                .Setup(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()))
                .ReturnsAsync(new HttpResponse<CreateImmediateBasketResponse>
                {
                    StatusCode = HttpStatusCode.OK, 
                    ResponseContent = new CreateImmediateBasketResponse
                    {
                        BasketReference = "testRef", 
                        BasketToken = "testBasketToken", 
                        ResponseCode = "00000"
                    }
                });

            _civicaPayGateway
                .Setup(_ => _.GetPaymentUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("redirectUrl");

            _paymentController.ObjectValidator = _objectValidator.Object;

            await _paymentController.Detail("slug", new PaymentSubmission
            {
                Payment = _processedPayment,
                Amount = 12.00m,
                Reference = "123456789"
            });

            _civicaPayGateway.Verify(_ => _.GetPaymentUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task SuccessShouldCallRepositoryForPaymentIfPathIsPayment()
        {
            await _paymentController.Success("slug", "callingAppTxnRef", "00000");

            _fakeRepository.Verify(_ => _.Get<Payment>("slug", null), Times.Once);
        }

        [Fact]
        public async Task SuccessShouldCallRepositoryForServicePayPaymentIfPathIsServicePayPayment()
        {
            await _paymentControllerWithServicePayPaymentPath.Success("slug", "callingAppTxnRef", "00000");

            _fakeRepository.Verify(_ => _.Get<ServicePayPayment>("slug", null), Times.Once);
        }

        [Fact]
        public async Task SuccessShouldReturnResponseIfNotSuccessful()
        {
            _fakeRepository
                .Setup(_ => _.Get<Payment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, _processedPayment, "Not found"));

            var result = await _paymentController.Success("slug", "callingAppTxnRef", "00000") as HttpResponse;

            result.Should().BeEquivalentTo(new HttpResponse((int) HttpStatusCode.NotFound, _processedPayment, "Not found"));
        }

        [Theory]
        [InlineData(true, "00022", "../ServicePayPayment/Declined")]
        [InlineData(true, "99999", "../ServicePayPayment/Failure")]
        [InlineData(false, "00023", "Declined")]
        [InlineData(false, "99999", "Failure")]
        public async Task SuccessShouldReturnCorrectErrorView(bool isServicePayPaymentPath, string responseCode, string expectedView)
        {
            var result = isServicePayPaymentPath 
                ? await _paymentControllerWithServicePayPaymentPath.Success("slug", "callingAppTxnRef", responseCode) as ViewResult
                : await _paymentController.Success("slug", "callingAppTxnRef", responseCode) as ViewResult;

            result.ViewName.Should().Be(expectedView);
        }

        [Fact]
        public async Task SuccessShouldReturnViewWithModel()
        {
            var model = new PaymentSuccess
            {
                Title = _processedPayment.Title,
                ReceiptNumber = "123456",
                MetaDescription = _processedPayment.MetaDescription
            };

            var result = await _paymentController.Success("slug", "123456", "00000") as ViewResult;

            result.Model.Should().BeEquivalentTo(model);
        }
    }
}
