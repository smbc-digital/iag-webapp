using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public record TriviaSection(string Heading, IEnumerable<Trivia> Trivia);
}
