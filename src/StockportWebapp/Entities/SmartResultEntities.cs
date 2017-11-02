using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StockportWebapp.Entities
{
    public class SmartResultEntity
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Subheading { get; set; }
        public string IconColour { get; set; }
        public string IconClass { get; set; }
        public string Body { get; set; }
        public string ButtonText { get; set; }
        public string ButtonLink { get; set; }
    }
}
