using System.Diagnostics.CodeAnalysis;

namespace StockportWebapp.ViewModels;

[Obsolete("Groups is being replaced by directories/directory entries")]
[ExcludeFromCodeCoverage(Justification = "Obsolete")]
public class ManageGroupViewModel
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public bool Administrator { get; set; }
    public bool IsArchived { get; set; }
}