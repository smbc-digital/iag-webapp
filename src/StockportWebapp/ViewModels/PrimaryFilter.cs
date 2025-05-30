﻿namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class PrimaryFilter
{
    public string Category { get; set; }
    public List<GroupCategory> Categories { set; get; }
    public string Order { get; set; }
    public List<string> Orders = new()
        {
            "Nearest",
            "Name A-Z",
            "Name Z-A"
        };
    
    public string Location { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}