using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Moq;
using StockportWebapp.ContentFactory;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;
using Xunit;
using FluentAssertions;

namespace StockportWebappTests_Unit.Unit.ContentFactory
{
    public class PaymentFactoryTest
    {
        private readonly Mock<MarkdownWrapper> _markdownWrapper;
        private readonly Mock<ISimpleTagParserContainer> _tagParserContainer;
        private readonly Payment _payment;
        private readonly PaymentFactory _factory;

        private readonly string Title = "Pay your council Tax";
        private readonly string Slug = "council-tax";
        private readonly string Description = "Description";
        private readonly string PaymentDetailsText = "Payment Details Text";
        private readonly string GlCodeCostCentreNumber = "1234";
        private readonly string ParisReference = "Paris reference";
        private readonly string Fund = "Fund";
        private readonly string ReferenceLabel = "Reference label";
        private readonly string MetaDescription = "Meta description";




        public PaymentFactoryTest()
        {
            _markdownWrapper = new Mock<MarkdownWrapper>();
            _tagParserContainer = new Mock<ISimpleTagParserContainer>();            
            _factory = new  PaymentFactory(_tagParserContainer.Object, _markdownWrapper.Object);
            _payment = new Payment()
            {
                Title = Title,
                Slug = Slug,                
                Description = Description,
                PaymentDetailsText = PaymentDetailsText,
                Fund = Fund,
                ReferenceLabel = ReferenceLabel,
                GlCodeCostCentreNumber = GlCodeCostCentreNumber,
                BreadCrumbs = new List<Crumb>(),
                MetaDescription = MetaDescription
            };

            _tagParserContainer.Setup(o => o.ParseAll(Description, It.IsAny<string>(), It.IsAny<bool>())).Returns(Description);
            _markdownWrapper.Setup(o => o.ConvertToHtml(Description)).Returns(Description);          
        }

        [Fact]
        public void ShouldSetTheCorrespondingFieldsForAProcessedPayment()
        {
            var result = _factory.Build(_payment);
            result.Title.Should().Be("Pay your council Tax");
            result.Description.Should().Be("Description");
            result.Slug.Should().Be("council-tax");
            result.MetaDescription.Should().Be("Meta description");

        }

        [Fact]
        public void ShouldProcessDescriptionWithMarkdown()
        {
            _factory.Build(_payment);

            _markdownWrapper.Verify(o => o.ConvertToHtml(Description), Times.Once);
        }

        [Fact]
        public void ShouldPassTitleToAllSimpleParsersWhenBuilding()
        {
            _factory.Build(_payment);

            _tagParserContainer.Verify(o => o.ParseAll(Description, _payment.Title, It.IsAny<bool>()), Times.Once);
        }
    }
}
