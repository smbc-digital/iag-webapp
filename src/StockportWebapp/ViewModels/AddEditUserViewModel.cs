using Microsoft.AspNetCore.Mvc.Rendering;
using StockportWebapp.Models;
using System.Collections.Generic;

namespace StockportWebapp.ViewModels
{
    public class AddEditUserViewModel
    {
        public string Slug { get; set; }
        public string Name { get; set; }
        public GroupAdministratorItems GroupAdministratorItem { get; set; }
        public List<SelectListItem> SelectList { get; set; }
        public string Previousrole { get; set; }

        public AddEditUserViewModel()
        {
            SelectList = new List<SelectListItem>();
            SelectList.Add(new SelectListItem() { Text = "Administrator", Value = "A" });
            SelectList.Add(new SelectListItem() { Text = "Editor", Value = "E" });
        }
    }
}
