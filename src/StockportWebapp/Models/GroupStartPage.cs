using StockportWebapp.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StockportWebapp.Utils;

namespace StockportWebapp.Models
{
    public class GroupStartPage
    {
        public List<GroupCategory> Categories = new List<GroupCategory>();

        public GroupStartPage() { }
        
    }
}
