using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockportWebapp.Models
{
    public class ContactUsId
    {
        public string Name { get; }
        public string Slug { get; }
        public string EmailAddress { get; }

        public ContactUsId(string name, string slug, string emailAddress)
        {
            Name = name;
            Slug = slug;
            EmailAddress = emailAddress;
        }
    }
}
