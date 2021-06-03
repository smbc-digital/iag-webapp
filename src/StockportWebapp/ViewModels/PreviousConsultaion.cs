using System.Collections.Generic;
using StockportWebapp.Models;

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
