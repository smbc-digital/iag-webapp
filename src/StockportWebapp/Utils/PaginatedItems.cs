using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Models;

namespace StockportWebapp.Utils
{
    public class PaginatedItems<T>
    {
        public List<T> Items { get; set; }
        public Pagination Pagination { get; set; }
    }
}