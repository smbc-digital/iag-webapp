using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ViewModels
{
    public class ConfirmationViewModel
    {
        public List<Crumb> Crumbs { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ConfirmationText { get; set; }
        public string ButtonText { get; set; }
        public string ButtonLink { get; set; }
        public string Icon { get; set; }
        public string IconColour { get; set; }
    }
}