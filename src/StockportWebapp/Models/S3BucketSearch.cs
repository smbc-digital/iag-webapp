using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockportWebapp.Models
{
    public class S3BucketSearch
    {
        public string Slug { get; set; }
        public string SearchTerm { get; set; }
        public List<string> Files { get; set; }
        public List<string> Folders { get; set; }
        public string S3Bucket { get; set; }
        public string AWSLink { get; set; }
        public string SearchFolder { get; set; }
    }
}
