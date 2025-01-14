using Org.BouncyCastle.Asn1.IsisMtt;

namespace StockportWebappTests_Unit.Unit.Utils;

public class QueryUrlTest
{
    [Fact]
    public void ShouldAddNewQueriesToQueryParameters()
    {
        // Arrange
        RouteValueDictionary startingRoutesDictionary = new() { { "name", "value" }, { "a-key", "a-value" } };
        Mock<IQueryCollection> mockQueryCollection = new();
        mockQueryCollection
            .Setup(query => query.Keys)
            .Returns(new List<string>() { "queryName" });
        
        mockQueryCollection
            .Setup(query => query["queryName"])
            .Returns("queryValue");

        // Act
        RouteValueDictionary routesDictionary = new QueryUrl(startingRoutesDictionary, mockQueryCollection.Object)
            .AddQueriesToUrl(new Dictionary<string, string> { { "newQueryName", "newQueryValue" } });

        // Assert
        Assert.Equal(4, routesDictionary.Count);
        Assert.Equal("value", routesDictionary["name"]);
        Assert.Equal("a-value", routesDictionary["a-key"]);
        routesDictionary["queryName"].Should().Be("queryValue");
        routesDictionary["newQueryName"].Should().Be("newQueryValue");
    }

    [Fact]
    public void ShouldRemoveQueriesFromQueryParamaters()
    {
        // Arrange
        RouteValueDictionary startingRoutesDictionary = new() { { "name", "value" } };
        Mock<IQueryCollection> mockQueryCollection = new();
        mockQueryCollection
            .Setup(query => query.Keys)
            .Returns(new List<string>() { "queryName", "anotherQueryName" });

        // Act
        RouteValueDictionary routesDictionary = new QueryUrl(startingRoutesDictionary, mockQueryCollection.Object)
            .WithoutQueryParam(new List<string>() { "queryName", "anotherQueryName" });

        // Assert
        Assert.Single(routesDictionary);
        Assert.Equal("value", routesDictionary["name"]);
    }

    [Fact]
    public void ShouldReturnFalseIfQueryIsInCurrentQueryParamaters()
    {
        // Arrange
        RouteValueDictionary startingRoutesDictionary = new() { { "name", "value" } };
        Mock<IQueryCollection> mockQueryCollection = new();
        mockQueryCollection
            .Setup(query => query.Keys)
            .Returns(new List<string>() { "queryName" });
        
        mockQueryCollection
            .Setup(query => query["queryName"])
            .Returns("queryValue");

        // Act
        bool isInQueryParameters = new QueryUrl(startingRoutesDictionary, mockQueryCollection.Object)
            .MatchesQueryParam("currentQueryName", "currentQueryValue");

        // Assert
        Assert.False(isInQueryParameters);
    }

    [Fact]
    public void ShouldReturnTrueIfQueryIsInCurrentQueryParamaters()
    {
        // Arrange
        RouteValueDictionary startingRoutesDictionary = new() { { "name", "value" } };
        Mock<IQueryCollection> mockQueryCollection = new();
        mockQueryCollection
            .Setup(query => query.ContainsKey("queryName"))
            .Returns(true);
        
        mockQueryCollection
            .Setup(query => query["queryName"])
            .Returns("queryValue");

        // Act
        bool isInQueryParameters = new QueryUrl(startingRoutesDictionary, mockQueryCollection.Object)
            .MatchesQueryParam("queryName", "queryValue");

        // Assert
        Assert.True(isInQueryParameters);
    }

    [Fact]
    public void ShouldReturnTrueIfQueryNameIsInCurrentQueryParamaters()
    {
        // Arrange
        RouteValueDictionary startingRoutesDictionary = new() { { "name", "value" } };
        Mock<IQueryCollection> mockQueryCollection = new();
        mockQueryCollection
            .Setup(query => query.ContainsKey("queryName"))
            .Returns(true);
        
        mockQueryCollection
            .Setup(query => query["queryName"])
            .Returns("queryValue");

        // Act
        bool isInQueryParameters = new QueryUrl(startingRoutesDictionary, mockQueryCollection.Object)
            .HasQueryParam("queryName");

        // Assert
        Assert.True(isInQueryParameters);
    }

    [Fact]
    public void ShouldReturnFalseIfQueryNameIsNotInCurrentQueryParamaters()
    {
        // Arrange
        RouteValueDictionary startingRoutesDictionary = new() { { "name", "value" } };
        Mock<IQueryCollection> mockQueryCollection = new();
        mockQueryCollection
            .Setup(query => query.ContainsKey("queryName"))
            .Returns(true);
        
        mockQueryCollection
            .Setup(query => query["queryName"])
            .Returns("queryValue");

        // Act
        bool isInQueryParameters = new QueryUrl(startingRoutesDictionary, mockQueryCollection.Object)
            .HasQueryParam("notInQueryString");

        // Assert
        Assert.False(isInQueryParameters);
    }
}