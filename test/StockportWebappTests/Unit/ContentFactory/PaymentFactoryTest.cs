using System;
using System.Collections.Generic;
using Moq;
using StockportWebapp.ContentFactory;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;
using Xunit;
using FluentAssertions;

namespace StockportWebappTests.Unit.ContentFactory
{
    public class PaymentFactoryTest
    {
        private readonly PaymentFactory _paymentFactory;
        private readonly Mock<MarkdownWrapper> _markdownWrapper;
        private readonly Mock<ISimpleTagParserContainer> _tagParserContainer;
        private readonly Mock<IDynamicTagParser<Document>> _documentTagParser;
    }
}
