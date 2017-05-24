using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StockportWebapp.Config;
using StockportWebapp.Http;
using HttpClient = System.Net.Http.HttpClient;

namespace StockportWebapp.Validation
{
    public class ValidateReCaptchaAttribute : ActionFilterAttribute
    {
        public const string ReCaptchaModelErrorKey = "ReCaptcha";
        private const string RecaptchaResponseTokenKey = "g-recaptcha-response";
        private const string ApiVerificationEndpoint = "https://www.google.com/recaptcha/api/siteverify";
        private readonly IApplicationConfiguration m_configuration;
        private readonly string _reCaptchaSecret;
        private readonly IHttpClient _httpClient;

        public ValidateReCaptchaAttribute(IApplicationConfiguration config, IHttpClient httpClient)
        {
            _reCaptchaSecret = config.GetReCaptchaKey().ToString();
            _httpClient = httpClient;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await DoReCaptchaValidation(context);

            await base.OnActionExecutionAsync(context, next);
        }

        private async Task DoReCaptchaValidation(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.HasFormContentType)
            {
                AddModelError(context, "No reCaptcha Token Found");
                return;
            }

            string token = context.HttpContext.Request.Form[RecaptchaResponseTokenKey];

            if (string.IsNullOrWhiteSpace(token))
            {
                AddModelError(context, "Verify you are not a robot by selecting the verification box above the \"submit\" button");
            }
            else
            {
                await ValidateRecaptcha(context, token);
            }
        }

        private static void AddModelError(ActionExecutingContext context, string error)
        {
            context.ModelState.AddModelError(ReCaptchaModelErrorKey, error.ToString());
        }

        private async Task ValidateRecaptcha(ActionExecutingContext context, string token)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("secret", _reCaptchaSecret),
                    new KeyValuePair<string, string>("response", token)
                });
            HttpResponseMessage response = await _httpClient.PostAsync(ApiVerificationEndpoint, content);
            string json = await response.Content.ReadAsStringAsync();
            var reCaptchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(json);
            if (reCaptchaResponse == null)
            {
                AddModelError(context, "Unable To Read Response From Server");
            }
            else if (!reCaptchaResponse.success)
            {
                AddModelError(context, "Invalid reCaptcha");
            }
            
        }
    }

    public class ReCaptchaResponse
    {
        public bool success { get; set; }
        public string challenge_ts { get; set; }
        public string hostname { get; set; }
        public string[] errorcodes { get; set; }
    }

}
