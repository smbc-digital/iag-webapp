using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests.Unit.Utils
{
    public class UrlQueryHelperTest
    {

        [Fact]
        public void ShouldAddNewQueryToQueryParamaters()
        {
            var startingRoutesDictionary = new RouteValueDictionary() { { "name", "value" } };
            var mockQueryCollection = new Mock<IQueryCollection>();
            mockQueryCollection.Setup(o => o.Keys).Returns(new List<string>() { "queryName"});
            mockQueryCollection.Setup(o => o["queryName"]).Returns("queryValue");

            var routesDictionary = UrlQueryHelper.AddQueryToUrl(startingRoutesDictionary, mockQueryCollection.Object,
                "newQueryName", "newQueryValue");

            routesDictionary.Count.Should().Be(3);
            routesDictionary["name"].Should().Be("value");
            routesDictionary["queryName"].Should().Be("queryValue");
            routesDictionary["newQueryName"].Should().Be("newQueryValue");
        }

        [Fact]
        public void ShouldRemoveQueryFromQueryParamaters()
        {
            var startingRoutesDictionary = new RouteValueDictionary() { { "name", "value" } };
            var mockQueryCollection = new Mock<IQueryCollection>();
            mockQueryCollection.Setup(o => o.Keys).Returns(new List<string>() { "queryName" });
            mockQueryCollection.Setup(o => o["queryName"]).Returns("queryValue");

            var routesDictionary = UrlQueryHelper.RemoveQueryToUrl(startingRoutesDictionary, mockQueryCollection.Object,
                "queryName");

            routesDictionary.Count.Should().Be(1);
            routesDictionary["name"].Should().Be("value");
        }

        [Fact]
        public void ShouldReturnFalseIfQueryIsInCurrentQueryParamaters()
        {
            var startingRoutesDictionary = new RouteValueDictionary() { { "name", "value" } };
            var mockQueryCollection = new Mock<IQueryCollection>();
            mockQueryCollection.Setup(o => o.Keys).Returns(new List<string>() { "queryName" });
            mockQueryCollection.Setup(o => o["queryName"]).Returns("queryValue");

            var isInQueryParameters = UrlQueryHelper.QueryNameAndValueIsInQueryString(startingRoutesDictionary, mockQueryCollection.Object,
                "currentQueryName", "currentQueryValue");

            isInQueryParameters.Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnTrueIfQueryIsInCurrentQueryParamaters()
        {
            var startingRoutesDictionary = new RouteValueDictionary() { { "name", "value" } };
            var mockQueryCollection = new Mock<IQueryCollection>();
            const string existingQueryName = "queryName";
            const string existingQueryValue = "queryValue";
            mockQueryCollection.Setup(o => o.ContainsKey(existingQueryName)).Returns(true);
            mockQueryCollection.Setup(o => o[existingQueryName]).Returns(existingQueryValue);

            var isInQueryParameters = UrlQueryHelper.QueryNameAndValueIsInQueryString(startingRoutesDictionary, mockQueryCollection.Object,
                existingQueryName, existingQueryValue);

            isInQueryParameters.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnTrueIfQueryNameIsInCurrentQueryParamaters()
        {
            var startingRoutesDictionary = new RouteValueDictionary() { { "name", "value" } };
            var mockQueryCollection = new Mock<IQueryCollection>();
            const string existingQueryName = "queryName";
            const string existingQueryValue = "queryValue";
            mockQueryCollection.Setup(o => o.ContainsKey(existingQueryName)).Returns(true);
            mockQueryCollection.Setup(o => o[existingQueryName]).Returns(existingQueryValue);

            var isInQueryParameters = UrlQueryHelper.QueryNameIsInQueryString(startingRoutesDictionary, mockQueryCollection.Object,
                existingQueryName);

            isInQueryParameters.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnFalseIfQueryNameIsNotInCurrentQueryParamaters()
        {
            var startingRoutesDictionary = new RouteValueDictionary() { { "name", "value" } };
            var mockQueryCollection = new Mock<IQueryCollection>();
            mockQueryCollection.Setup(o => o.ContainsKey("queryName")).Returns(true);
            mockQueryCollection.Setup(o => o["queryName"]).Returns("queryValue");

            var isInQueryParameters = UrlQueryHelper.QueryNameIsInQueryString(startingRoutesDictionary, mockQueryCollection.Object,
                "notInQueryString");

            isInQueryParameters.Should().BeFalse();
        }
    }
}
