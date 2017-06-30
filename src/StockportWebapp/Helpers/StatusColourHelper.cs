using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockportWebapp.Models;

namespace StockportWebapp.Utils
{
    public static class StatusColourHelper
    {
        public static string GetStatusColour(string status)
        {
            switch (status)
            {
                case "Published":
                    return "green";
                case "Archived":
                    return "red";
                default:
                    return "green";
            }
        }
    }
}
