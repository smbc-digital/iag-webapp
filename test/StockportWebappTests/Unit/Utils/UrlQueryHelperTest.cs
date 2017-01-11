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
        public void ShouldAddNewQueriesToQueryParamaters()
        {
            var startingRoutesDictionary = new RouteValueDictionary() { { "name", "value" }, { "a-key", "a-value" } };
            var mockQueryCollection = new Mock<IQueryCollection>();
            mockQueryCollection.Setup(o => o.Keys).Returns(new List<string>() { "queryName"});
            mockQueryCollection.Setup(o => o["queryName"]).Returns("queryValue");

            var routesDictionary = new UrlQueryHelper(startingRoutesDictionary, mockQueryCollection.Object).AddQueriesToUrl(new Dictionary<string, string> { { "newQueryName", "newQueryValue"} });

            routesDictionary.Count.Should().Be(4);
            routesDictionary["name"].Should().Be("value");
            routesDictionary["queryName"].Should().Be("queryValue");
            routesDictionary["newQueryName"].Should().Be("newQueryValue");
            routesDictionary["a-key"].Should().Be("a-value");
        }

        [Fact]
        public void ShouldRemoveQueriesFromQueryParamaters()
        {
            var startingRoutesDictionary = new RouteValueDictionary() { { "name", "value" } };
            var mockQueryCollection = new Mock<IQueryCollection>();
            mockQueryCollection.Setup(o => o.Keys).Returns(new List<string>() { "queryName", "anotherQueryName" });

            var routesDictionary = new  UrlQueryHelper(startingRoutesDictionary, mockQueryCollection.Object).RemoveQueriesFromUrl(new List<string>() { "queryName" , "anotherQueryName" });

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

            var isInQueryParameters = new UrlQueryHelper(startingRoutesDictionary, mockQueryCollection.Object).QueryNameAndValueIsInQueryString("currentQueryName", "currentQueryValue");

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

            var isInQueryParameters = new UrlQueryHelper(startingRoutesDictionary, mockQueryCollection.Object)
                .QueryNameAndValueIsInQueryString(existingQueryName, existingQueryValue);

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

            var isInQueryParameters = new UrlQueryHelper(startingRoutesDictionary, mockQueryCollection.Object).
                QueryNameIsInQueryString(existingQueryName);

            isInQueryParameters.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnFalseIfQueryNameIsNotInCurrentQueryParamaters()
        {
            var startingRoutesDictionary = new RouteValueDictionary() { { "name", "value" } };
            var mockQueryCollection = new Mock<IQueryCollection>();
            mockQueryCollection.Setup(o => o.ContainsKey("queryName")).Returns(true);
            mockQueryCollection.Setup(o => o["queryName"]).Returns("queryValue");

            var isInQueryParameters = new UrlQueryHelper(startingRoutesDictionary, mockQueryCollection.Object).QueryNameIsInQueryString("notInQueryString");

            isInQueryParameters.Should().BeFalse();
        }
    }
}
