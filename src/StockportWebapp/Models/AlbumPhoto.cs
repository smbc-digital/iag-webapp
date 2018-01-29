using System;
using System.Collections.Generic;
namespace StockportWebapp.Models
{
    public class AlbumPhoto
    {
        public decimal albumphotoidno { get; set; }
        public decimal albumidno { get; set; }
        public string photograph { get; set; }
        public string caption { get; set; }
        public string dateadded { get; set; }
        public string status { get; set; }
        public string photoPath { get; set; }

    }
}