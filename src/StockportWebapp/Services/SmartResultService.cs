using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockportWebapp.Entities;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Wrappers;

namespace StockportWebapp.Services
{
    public interface ISmartResultService
    {
        Task<SmartResultEntity> GetSmartResult(string slug);
    }

    public class SmartResultService : ISmartResultService
    {
        private readonly ISmartResultRepository _repository;
        private readonly ILogger<SmartResultService> _logger;
        private readonly IHttpClientWrapper _httpClient;

        public SmartResultService(ISmartResultRepository repository, ILogger<SmartResultService> logger, IHttpClientWrapper httpClient)
        {
            _repository = repository;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<SmartResultEntity> GetSmartResult(string slug)
        {
            var model = await _repository.GetSmartResult(slug);

            var result = new SmartResultEntity
            {
                Title = model.Title,
                ButtonLink = model.ButtonLink,
                ButtonText = model.ButtonText,
                Body = model.Body,
                IconClass = GetIconClass(model.Icon),
                IconColour = GetIconColour(model.Icon),
                Slug = model.Slug,
                Subheading = model.Subheading
            };

            return result;
        }

        private static string GetIconClass(string icon)
        {
            switch (icon?.ToUpper())
            {
                case "EXCLAMATION MARK":
                    return "exclamation";
                case "TICK":
                    return "check";
                case "CROSS":
                    return "times";
                default:
                    return "times";
            }
        }

        private static string GetIconColour(string icon)
        {
            switch (icon?.ToUpper())
            {
                case "EXCLAMATION MARK":
                    return "red";
                case "TICK":
                    return "green";
                case "CROSS":
                    return "red";
                default:
                    return "green";
            }
        }

    }
}

