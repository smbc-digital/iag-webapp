using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StockportWebapp.Models
{
    public class CheckBoxItem
    {
        public string Name { get; set; }
        
        public bool IsSelected { get; set; }
    }
}
