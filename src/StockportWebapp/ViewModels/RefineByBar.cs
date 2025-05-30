﻿namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class RefineByFilterItems
{
    public string Label { get; set; }
    public string Value { get; set; }
    public bool Checked { get; set; }
}

[ExcludeFromCodeCoverage]
public class RefineByFilters
{
    public string Label { get; set; }
    public string Name { get; set; }
    public bool Mandatory { get; set; }
    public List<RefineByFilterItems> Items { get; set; }
}

[ExcludeFromCodeCoverage]
public class RefineByBar
{
    public bool ShowLocation { get; set; }
    public bool KeepLocationQueryValues { get; set; }
    public List<RefineByFilters> Filters { get; set; }
    public string MobileFilterText { get; set; } = "Refine by";
}