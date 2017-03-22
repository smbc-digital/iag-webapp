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
        ParisRecordXML parisRecordXML;
        string parisRecordXMLStringOutput;

        public ParisLinkBuilderTest()
        {
            _parisLinkBuilder = new ParisLinkBuilder();
            parisRecordXML = new ParisRecordXML() { amount = "amount", fund = "fund", reference = "reference", text6 = "text6" };
            parisRecordXMLStringOutput = "<records><record><reference>reference</reference><fund>fund</fund><amount>amount</amount><text6>text6</text6></record></records>";
        }

        [Fact]
        public void ShouldReturnXMLWhenReferencesObjectProvided()
        {
            

            var parisLink = _parisLinkBuilder.ParisRecordXML(parisRecordXML).Build();

            parisLink.Should().NotBeNull();

            parisLink.Should().Contain(parisRecordXMLStringOutput);
        }

        [Fact]
        public void ShouldReturnEmptyValuesWhenNoneAreProvided()
        {
            var parisLink = _parisLinkBuilder.Build();

            parisLink.Should().EndWith("?returntext=&ignoreconfirmation=&payforbasketmode=&data=&recordxml=&returnurl=");
        }

        [Fact]
        public void ShouldReturnFullQueryStringWhenAllValuesAreSet()
        {
            var parisLink = _parisLinkBuilder.PayForBasketMode("True")
                                             .IgnoreConfirmation("True")
                                             .Data("Data")
                                             .ParisRecordXML(parisRecordXML)
                                             .ReturnText("Test")
                                             .ReturnUrl("Test")
                                             .Build();

            parisLink.Should().EndWith("?returntext=Test&ignoreconfirmation=True&payforbasketmode=True&data=Data&recordxml=" + parisRecordXMLStringOutput + "&returnurl=Test");
        }
    }
}
