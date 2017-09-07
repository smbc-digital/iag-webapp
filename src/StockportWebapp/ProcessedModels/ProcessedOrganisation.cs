using System.Collections.Generic;
using System.Linq;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;

namespace StockportWebapp.ProcessedModels
{

    public class ProcessedOrganisation : IProcessedContentType
    {
        public string Title { get;  }
        public string Slug { get;  }
        public string ImageUrl { get;  }
        public string AboutUs { get;  }
        public string Phone { get;  }
        public string Email { get;  }
        public bool Volunteering { get;  }
        public string VolunteeringText { get;  }

        public ProcessedOrganisation() { }

        public ProcessedOrganisation(string title, string slug, string imageUrl, string aboutUs, string phone,
            string email, bool volunteering, string volunteeringText)
        {
            Title = title;
            Slug = slug;
            Phone = phone;
            Email = email;
            AboutUs = aboutUs;
            ImageUrl = imageUrl;
            Volunteering = volunteering;
            VolunteeringText = volunteeringText;
        }
    }
}
