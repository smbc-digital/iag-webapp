using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Models;
using StockportWebapp.ViewComponents;

namespace StockportWebappTests.Unit.ViewComponents
{
    public class HomepageEventsViewComponentsTest
    {
        [Fact]
        public void ShouldReturnHomepageEventsWithListingIfFeatureToggleIsOn()
        {
            var viewComponents = new HomepageEventsViewComponent(new FeatureToggles {LatestEventsHomepage = true});
            var result = AsyncTestHelper.Resolve(viewComponents.InvokeAsync(new List<Event>())) as ViewViewComponentResult;

            result.ViewData.Model.Should().BeOfType<List<Event>>();
            result.ViewName.Should().Be("HomepageEventsWithListing");
        }

        [Fact]
        public void ShouldReturnHomepageEventsWithoutListingIfFeatureToggleIsOff()
        {
            var viewComponents = new HomepageEventsViewComponent(new FeatureToggles { LatestEventsHomepage = false });
            var result = AsyncTestHelper.Resolve(viewComponents.InvokeAsync(new List<Event>())) as ViewViewComponentResult;

            result.ViewData.Model.Should().Be(null);
            result.ViewName.Should().Be("HomepageEventsWithoutListing");
        }
    }
}
