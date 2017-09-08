using StockportWebapp.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StockportWebapp.Utils;

namespace StockportWebapp.Models
{
    public class GroupHomepage
    {
        public string Title { get; set; }
        public string BackgroundImage { get; set; }

        public GroupHomepage() { }
        
    }
}
