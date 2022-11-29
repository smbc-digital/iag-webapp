using Microsoft.AspNetCore.Mvc.Rendering;
using StockportWebapp.Models;

namespace StockportWebapp.ViewModels
{
    public class SIAViewModel
    {

        public List<Photo> Photos { get; set; }
        public List<SIAArea> Areas { get; set; }

        public string SelectedArea { get; set; }
        public SelectList AreaList { get; set; }

        public string SearchDepth { get; set; }

        public List<AlbumInfo> Albums { get; set; }


    }
}
