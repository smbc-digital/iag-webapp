using FluentAssertions;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests.Unit.Model
{
    public class MarkdownExampleTest
    {
        [Fact]
        public void ConvertsTableMarkdownToHtmlTable()
        {
            var body = "# Headings are fun\r\n" +
                       "| Tables are great | 1      | 2      | 3          | 4    |\r\n" +
                       "|------------------|--------|--------|------------|------|\r\n" +
                       "| White space is removed     | OneOne | Two    | Three      | Four |\r\n" +
                       "|    No matter where it is|    One | TwoTwo |      Three | Four |\r\n";

            var convertedBody = MarkdownWrapper.ToHtml(body);

            var expected = "<h1>Headings are fun</h1>\n" +
                           "<div class=\"table\">\n" +
                           "<table>\n" +
                           "<thead>\n" +
                           "<tr>\n<th>Tables are great</th>\n<th>1</th>\n<th>2</th>\n<th>3</th>\n<th>4</th>\n</tr>\n" +
                           "</thead>\n" +
                           "<tbody>\n" +
                           "<tr>\n<td>White space is removed</td>\n<td>OneOne</td>\n<td>Two</td>\n<td>Three</td>\n<td>Four</td>\n</tr>\n" +
                           "<tr>\n<td>No matter where it is</td>\n<td>One</td>\n<td>TwoTwo</td>\n<td>Three</td>\n<td>Four</td>\n</tr>\n" +
                           "</tbody>\n" +
                           "</table>\n" +
                           "</div>\n";
            Assert.Equal(expected, convertedBody);
        }

        [Fact]
        public void ConvertsTableMarkdownToHtmlTableWithInstanceOfMarkdownWrapper()
        {
            var body = "# Headings are fun\r\n" +
                       "| Tables are great | 1      | 2      | 3          | 4    |\r\n" +
                       "|------------------|--------|--------|------------|------|\r\n" +
                       "| White space is removed     | OneOne | Two    | Three      | Four |\r\n" +
                       "|    No matter where it is|    One | TwoTwo |      Three | Four |\r\n";

            var convertedBody = new MarkdownWrapper().ConvertToHtml(body);

            var expected = "<h1>Headings are fun</h1>\n" +
                           "<div class=\"table\">\n" +
                           "<table>\n" +
                           "<thead>\n" +
                           "<tr>\n<th>Tables are great</th>\n<th>1</th>\n<th>2</th>\n<th>3</th>\n<th>4</th>\n</tr>\n" +
                           "</thead>\n" +
                           "<tbody>\n" +
                           "<tr>\n<td>White space is removed</td>\n<td>OneOne</td>\n<td>Two</td>\n<td>Three</td>\n<td>Four</td>\n</tr>\n" +
                           "<tr>\n<td>No matter where it is</td>\n<td>One</td>\n<td>TwoTwo</td>\n<td>Three</td>\n<td>Four</td>\n</tr>\n" +
                           "</tbody>\n" +
                           "</table>\n" +
                           "</div>\n";
            Assert.Equal(expected, convertedBody);
        }

        [Fact]
        public void ShouldAddHardBreakForSoftLineBreaks()
        {
            var body = "this is a body \n and so is this";
            var convertedBody = new MarkdownWrapper().ConvertToHtml(body);
            var expected = "<p>this is a body<br />\nand so is this</p>\n";

            convertedBody.Should().Be(expected);
        }
    }
}