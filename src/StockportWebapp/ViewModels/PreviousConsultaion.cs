using StockportWebapp.Models;
using System.Collections.Generic;

namespace StockportWebapp.ViewModels
{
    public class PreviousConsultaion
    {
        public Pagination Pagination { get; set; }
        public IEnumerable<Consultation> Consultations { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
    }
}
