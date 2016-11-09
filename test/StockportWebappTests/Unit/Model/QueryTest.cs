using FluentAssertions;
using StockportWebapp.Models;
using Xunit;

namespace StockportWebappTests.Unit.Model
{
    public class QueryTest
    {
        [Fact]
        public void ShouldCreateFormattedQueryOnToString()
        {
            const string name = "name";
            const string value = "value";
            var query = new Query(name, value);

            query.ToString().Should().Be($"{ name }={ value }");
        }
    }
}
