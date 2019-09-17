using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace StockportWebapp.QuestionBuilder.Entities
{
    public class InlineAlert
    {
        public string Icon { get; set; }

        public string Content { get; set; }
    }
}
