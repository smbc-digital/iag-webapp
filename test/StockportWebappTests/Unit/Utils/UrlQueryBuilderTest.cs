using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests.Unit.Utils
{
    public class UrlQueryBuilderTest
    {

        [Fact]
        public void ShouldAddNewQueryToQueryParamaters()
        {
            var startingRoutesDictionary = new RouteValueDictionary() { { "name", "value" } };
            var mockQueryCollection = new Mock<IQueryCollection>();
            mockQueryCollection.Setup(o => o.Keys).Returns(new List<string>() { "queryName"});
            mockQueryCollection.Setup(o => o["queryName"]).Returns("queryValue");

            var routesDictionary = UrlQueryBuilder.AddQueryToUrl(startingRoutesDictionary, mockQueryCollection.Object,
                "newQueryName", "newQueryValue");

            routesDictionary.Count.Should().Be(3);
            routesDictionary["name"].Should().Be("value");
            routesDictionary["queryName"].Should().Be("queryValue");
            routesDictionary["newQueryName"].Should().Be("newQueryValue");
        }
    }
}
