using System;
using System.Collections.Generic;
using System.Text;
using StockportWebapp.Models;

namespace StockportWebappTests.Builders
{
    public class DocumentBuilder
    {
        private string _assetId = "asset id";
        private string _title = "title";
        private int _size = 22;
        private string _url = "url";
        private DateTime _lastUpdated = DateTime.MinValue;
        private string _fileName = "fileName";
        private string _mediaType = "media";

        public Document Build()
        {
            return new Document(_title, _size, _lastUpdated, _url, _fileName, _assetId, _mediaType);
        }
    }
}
