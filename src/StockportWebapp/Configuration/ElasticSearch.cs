using Serilog.Events;

namespace StockportWebapp.Configuration
{
    public class ElasticSearch
    {
        public string Url { get; set; }
        public string LogFormat { get; set; }
        public string Authorization { get; set; }
        public LogEventLevel LogLevel { get; set; }
        public bool Enabled { get; set; }
    }
}
