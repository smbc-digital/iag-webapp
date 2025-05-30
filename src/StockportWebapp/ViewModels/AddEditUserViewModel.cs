﻿namespace StockportWebapp.ViewModels;

public class AddEditUserViewModel
{
    public string Slug { get; set; }
    public string Name { get; set; }
    public GroupAdministratorItems GroupAdministratorItem { get; set; }
    public List<SelectListItem> SelectList { get; set; }
    public string Previousrole { get; set; }

    public AddEditUserViewModel()
    {
        SelectList = new List<SelectListItem>()
        {
            new() { Text = "Administrator", Value = "A" },
            new() { Text = "Editor", Value = "E" }
        };
    }
}