using StockportWebapp.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StockportWebapp.Utils;

namespace StockportWebapp.Models
{
    public class GroupCategory
    {
        public string Name { get; set; }      
        public string Icon { get; set; }    

        public GroupCategory() { }
        
    }
}
