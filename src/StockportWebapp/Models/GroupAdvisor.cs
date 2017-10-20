using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockportWebapp.Models
{
    public class GroupAdvisor
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public IEnumerable<string> Groups { get; set; }
        public bool HasGlobalAccess { get; set; }
    }
}
