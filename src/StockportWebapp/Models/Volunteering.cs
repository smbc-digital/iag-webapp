using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockportWebapp.Models
{
    public class Volunteering
    {
        public string VolunteeringText { get; set; }
        public bool VolunteeringNeeded { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
    }
}
