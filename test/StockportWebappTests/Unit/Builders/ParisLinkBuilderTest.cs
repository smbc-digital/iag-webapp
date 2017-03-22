using System.Collections.Generic;
using System.Text;
using System.IO;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using StockportWebapp.Builders;
using StockportWebapp.Models;

namespace StockportWebappTests.Unit.Builders
{
    public class ParisLinkBuilderTest
    {
        private readonly IParisLinkBuilder _parisLinkBuilder;

        public ParisLinkBuilderTest()
        {
            _parisLinkBuilder = new ParisLinkBuilder();
        }

        [Fact]
        public void ShouldReturnXMLWhenReferencesObjectProvided()
        {
            var parisRecordXML = new ParisRecordXML() { amount = "amount", fund = "fund", reference = "reference", text6 = "text6" };

            var parisLink = _parisLinkBuilder.ParisRecordXML(parisRecordXML).Build();

            parisLink.Should().NotBeNull();

            parisLink.Should().Contain("<records><record><reference>reference</reference><fund>fund</fund><amount>amount</amount><text6>text6</text6></record></records>");
        }
    }
}
