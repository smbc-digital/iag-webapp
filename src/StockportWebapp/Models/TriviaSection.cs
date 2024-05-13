using System.Diagnostics.CodeAnalysis;

namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public record TriviaSection(string Heading, IEnumerable<Trivia> Trivia);
