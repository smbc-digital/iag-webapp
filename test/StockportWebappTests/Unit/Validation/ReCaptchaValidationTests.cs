﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using MimeKit.Cryptography;
using Moq;
using Org.BouncyCastle.Asn1.Mozilla;
using StockportWebapp.Config;
using StockportWebapp.Http;
using StockportWebapp.Validation;
using StockportWebappTests_Unit.Unit.Http;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Validation
{
    public class ReCaptchaValidationTests
    {
        public Mock<IApplicationConfiguration> _config = new Mock<IApplicationConfiguration>();
        private Mock<IHttpClient> _httpClient;
        private ValidateReCaptchaAttribute validationMethod;
        private ModelStateDictionary modelState;
        private ActionExecutingContext actionExcecutingContext;

        public ReCaptchaValidationTests()
        {
            _httpClient = new Mock<IHttpClient>();
            _config.Setup(x => x.GetReCaptchaKey()).Returns(AppSetting.GetAppSetting("recaptchakey"));
        }

        [Fact]
        public void ShouldReturnValidationSuccessWhenTokenIsCorrect()
        {
            // Arrange
            SetUpParameters();
            var responseMessage = new HttpResponseMessage();
            responseMessage.Content = new StringContent("{\"success\": true,\"challenge_ts\": \"2017-05-23T15:50:16Z\",\"hostname\": \"stockportgov.local\"}");

            _httpClient.Setup(
                    x => x.PostRecaptchaAsync("https://www.google.com/recaptcha/api/siteverify", It.IsAny<FormUrlEncodedContent>()))
                .ReturnsAsync(responseMessage);

            // Act
            var response = validationMethod.OnActionExecutionAsync(actionExcecutingContext, null);

            // Assert
            modelState.IsValid.Should().Be(true);
        }

        [Fact]
        public void ShouldReturnValidationFailureWhenTokenIsIncorrect()
        {
            // Arrange
            SetUpParameters();
            var responseMessage = new HttpResponseMessage();
            responseMessage.Content = new StringContent("{\"success\": false,\"challenge_ts\": \"2017-05-23T15:50:16Z\",\"hostname\": \"stockportgov.local\"}");

            _httpClient.Setup(
                    x => x.PostRecaptchaAsync("https://www.google.com/recaptcha/api/siteverify", It.IsAny<FormUrlEncodedContent>()))
                .ReturnsAsync(responseMessage);

            // Act
            var response = validationMethod.OnActionExecutionAsync(actionExcecutingContext, null);

            // Assert
            modelState.IsValid.Should().Be(false);
        }

        private void SetUpParameters()
        {
            validationMethod = new ValidateReCaptchaAttribute(_config.Object, _httpClient.Object);
            modelState = new ModelStateDictionary();
            var httpContenxt = new DefaultHttpContext();

            Dictionary<string, StringValues> formRecaptchaToken = new Dictionary<string, StringValues>();
            formRecaptchaToken.Add("g-recaptcha-response", "testValue");

            httpContenxt.Request.Form = new FormCollection(formRecaptchaToken);

            var actionContext = new ActionContext(
                httpContenxt,
                new Mock<RouteData>().Object,
                new Mock<ActionDescriptor>().Object,
                modelState);

            actionExcecutingContext = new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object>(),
            new Mock<Controller>());
        }
    }
}
