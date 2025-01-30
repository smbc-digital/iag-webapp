namespace StockportWebappTests_Unit.Unit.Utils;

public class ContentSecurityPolicyElementTest
{
    [Fact]
    public void CspElementContainsSpecifiedSourceType()
    {
        // Arrange
        ContentSecurityPolicyElement cspElement = new("specified-source-type");

        // Act
        string elementString = cspElement.Finish();

        // Assert
        Assert.Contains("specified-source-type", elementString);
    }

    [Fact]
    public void CspElementContainsSelfByDefault()
    {
        // Arrange
        ContentSecurityPolicyElement cspElement = new("source");

        // Act
        string elementString = cspElement.Finish();

        // Assert
        Assert.Contains(" 'self'", elementString);
    }

    [Fact]
    public void CspElementDoesNotContainSelfIfRequestedNotTo()
    {
        // Arrange
        ContentSecurityPolicyElement cspElement = new("source", containsSelf: false);

        // Act
        string elementString = cspElement.Finish();

        // Assert
        Assert.DoesNotContain(" 'self'", elementString);
    }

    [Fact]
    public void CspElementContainsSpecifiedSource()
    {
        // Arrange
        ContentSecurityPolicyElement cspElement = new("sourceType");

        // Act
        string elementString = cspElement
            .AddSource("specified-source")
            .Finish();

        // Assert
        Assert.Contains("specified-source", elementString);
    }

    [Fact]
    public void CspElementAddsHttpAndHttpsToSource()
    {
        // Arrange
        ContentSecurityPolicyElement cspElement = new("sourceType");

        // Act
        string elementString = cspElement
            .AddSource("specified-source")
            .Finish();

        // Assert
        Assert.True(elementString.Contains("http://specified-source") && elementString.Contains("https://specified-source"));
    }

    [Fact]
    public void CspElementDoesNotAddHttpAndHttpsToUnsafeInline()
    {
        // Arrange
        ContentSecurityPolicyElement cspElement = new("sourceType");

        // Act
        string elementString = cspElement
            .AddSource("'unsafe-inline'")
            .Finish();

        // Assert
        Assert.True(elementString.Contains("'unsafe-inline'")
            && !elementString.Contains("http://'unsafe-inline'")
            && !elementString.Contains("https://'unsafe-inline'"));
    }

    [Fact]
    public void CspElementDoesNotAddHttpAndHttpsToUnsafeEval()
    {
        // Arrange
        ContentSecurityPolicyElement cspElement = new("sourceType");

        // Act
        string elementString = cspElement
            .AddSource("'unsafe-eval'")
            .Finish();

        // Assert
        Assert.True(elementString.Contains("'unsafe-eval'")
            && !elementString.Contains("http://'unsafe-eval'")
            && !elementString.Contains("https://'unsafe-eval'"));
    }

    [Fact]
    public void CspElementDoesNotAddHttpAndHttpsToData()
    {
        // Arrange
        ContentSecurityPolicyElement cspElement = new("sourceType");

        // Act
        string elementString = cspElement
            .AddSource("data:")
            .Finish();

        // Assert
        Assert.True(elementString.Contains("data:")
            && !elementString.Contains("http://data:")
            && !elementString.Contains("https://data:"));
    }

    [Fact]
    public void CspElementDoesNotAddHttpAndHttpsToSourceStartingWithWildcard()
    {
        // Arrange
        ContentSecurityPolicyElement cspElement = new("sourceType");

        // Act
        string elementString = cspElement
            .AddSource("*.source.com")
            .Finish();

        // Assert
        Assert.True(elementString.Contains("*.source.com")
            && !elementString.Contains("http://*.source.com")
            && !elementString.Contains("https://*.source.com"));
    }

    [Fact]
    public void CspElementDoesNotAddHttpAndHttpsToHttps()
    {
        // Arrange
        ContentSecurityPolicyElement cspElement = new("sourceType");

        // Act
        string elementString = cspElement
            .AddSource("https:")
            .Finish();

        // Assert
        Assert.True(elementString.Contains("https:")
            && !elementString.Contains("http://https:")
            && !elementString.Contains("https://https:"));
    }

    [Fact]
    public void CspElementDoesNotDuplicateHttpIfSourceAlreadyHasIt()
    {
        // Arrange
        ContentSecurityPolicyElement cspElement = new("sourceType");

        // Act
        string elementString = cspElement
            .AddSource("http://specified-source")
            .Finish();

        // Assert
        Assert.True(elementString.Contains("http://specified-source")
            && !elementString.Contains("http://http://specified-source"));
    }

    [Fact]
    public void CspElementDoesNotDuplicateHttpsIfSourceAlreadyHasIt()
    {
        // Arrange
        ContentSecurityPolicyElement cspElement = new("sourceType");

        // Act
        string elementString = cspElement
            .AddSource("https://specified-source")
            .Finish();

        // Assert
        Assert.True(elementString.Contains("https://specified-source")
            && !elementString.Contains("https://https://specified-source"));
    }

    [Fact]
    public void CspElementAddsHttpsIfSourceAlreadyHasHttp()
    {
        // Arrange
        ContentSecurityPolicyElement cspElement = new("sourceType");

        // Act
        string elementString = cspElement
            .AddSource("http://raw-source")
            .Finish();

        // Assert
        Assert.True(elementString.Contains("http://raw-source")
            && elementString.Contains("https://raw-source"));
    }

    [Fact]
    public void CspElementAddsHttpIfSourceAlreadyHasHttps()
    {
        // Arrange
        ContentSecurityPolicyElement cspElement = new("sourceType");

        // Act
        string elementString = cspElement
            .AddSource("https://raw-source")
            .Finish();

        // Assert
        Assert.True(elementString.Contains("https://raw-source")
            && elementString.Contains("http://raw-source"));
    }

    [Fact]
    public void CspElementEndsWithSemiColonAndSpace()
    {
        // Arrange
        ContentSecurityPolicyElement cspElement = new("source-type");

        // Act
        string elementString = cspElement.Finish();

        // Assert
        Assert.EndsWith("; ", elementString);
    }

    [Fact]
    public void CspElementContainsAllPartsInCorrectOrder()
    {
        // Arrange
        string expectedElement =
            "source-type 'self'" +
            " http://source1.com https://source1.com" +
            " http://source2.com https://source2.com" +
            " http://source3.com https://source3.com" +
            " *.source4.com" +
            " data:" +
            " https:" +
            " 'unsafe-eval'" +
            " 'unsafe-inline'" +
            "; ";

        // Act
        string elementString = new ContentSecurityPolicyElement("source-type")
            .AddSource("http://source1.com")
            .AddSource("https://source2.com")
            .AddSource("source3.com")
            .AddSource("*.source4.com")
            .AddSource("data:")
            .AddSource("https:")
            .AddSource("'unsafe-eval'")
            .AddSource("'unsafe-inline'")
            .Finish();

        // Assert
        Assert.Equal(expectedElement, elementString);
    }
}