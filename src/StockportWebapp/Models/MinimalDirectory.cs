using System.Diagnostics.CodeAnalysis;

namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public readonly record struct MinimalDirectory(string Slug, string Title);