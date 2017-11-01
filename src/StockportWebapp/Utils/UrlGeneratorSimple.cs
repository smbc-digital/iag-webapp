using System;
using System.Collections.Generic;
using StockportWebapp.Config;
using StockportWebapp.Models;

namespace StockportWebapp.Utils
{
    public interface IUrlGeneratorSimple
    {
        string BaseContentApiUrl<T>();
        string StockportApiUrl<T>();
    }

    public class UrlGeneratorSimple : IUrlGeneratorSimple
    {
        private readonly IApplicationConfiguration _config;
        private readonly BusinessId _businessId;

        private readonly Dictionary<Type, string> _urls = new Dictionary<Type, string>()
        {
            {typeof(List<Event>), "events"},
            {typeof(Document), "documents"}
        };

        public UrlGeneratorSimple(IApplicationConfiguration config, BusinessId businessId)
        {
            _config = config;
            _businessId = businessId;
        }

        public string BaseContentApiUrl<T>()
        {
            return string.Concat(_config.GetContentApiUri(), _businessId, "/", _urls[typeof(T)], "/");
        }

        public string StockportApiUrl<T>()
        {
            return string.Concat(_config.GetStockportApiUri(), _urls[typeof(T)], "/");
        }
    }
}