﻿namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class EventCategory
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Icon { get; set; }
    public string Image { get; set; }

    public EventCategory() { }
}