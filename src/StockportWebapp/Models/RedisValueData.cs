using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockportWebapp.Models
{
    public class RedisValueData
    {
        public string Key { get; set; }
        public string Expiry { get; set; }
        public int NumberOfItems { get; set; }
    }
}
