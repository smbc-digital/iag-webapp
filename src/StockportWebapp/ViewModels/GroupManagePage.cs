using System;
using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ViewModels
{
    public class GroupManagePage
    {
        public List<Group> Groups { get; set; }
        public string Email { get; set; }
        public string ContactPageUrl { get; set; }
    }
}
