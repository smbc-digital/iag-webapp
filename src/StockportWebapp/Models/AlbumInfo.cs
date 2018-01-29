using System;
using System.Collections.Generic;
namespace StockportWebapp.Models
{
    public class AlbumInfo
    {
        public decimal albumidno { get; set; }
        public string albumtitle { get; set; }
        public string albumdescription { get; set; }
        public string createdby { get; set; }
        public string datecreated { get; set; }
        public string status { get; set; }
        public string albumcover { get; set; }
        public List<AlbumPhoto> AlbumPhotos { get; set; }
    }
}
