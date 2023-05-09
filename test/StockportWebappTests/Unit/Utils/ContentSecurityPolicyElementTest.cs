namespace StockportWebappTests_Unit.Unit.Utils;

public class ContentSecurityPolicyElementTest
{
    [Fact]
    public void CspElementContainsSpecifiedSourceType()
    {
        // Arrange
        string specifiedSourceType = "specified-source-type";
        var cspElement = new ContentSecurityPolicyElement(specifiedSourceType);

        // Act
        var elementString = cspElement.Finish();

        // Assert
        Assert.Contains(specifiedSourceType, elementString);
    }

    [Fact]
    public void CspElementContainsSelfByDefault()
    {
        // Arrange
        var cspElement = new ContentSecurityPolicyElement("source");

        // Act
        var elementString = cspElement.Finish();

        // Assert
        Assert.Contains(" 'self'", elementString);
    }

    [Fact]
    public void CspElementDoesNotContainSelfIfRequestedNotTo()
    {
        // Arrange
        var cspElement = new ContentSecurityPolicyElement("source", containsSelf: false);

        // Act
        var elementString = cspElement.Finish();

        // Assert
        Assert.DoesNotContain(" 'self'", elementString);
    }

    [Fact]
    public void CspElementContainsSpecifiedSource()
    {
        // Arrange
        string specifiedSource = "specified-source";
        var cspElement = new ContentSecurityPolicyElement("sourceType");

        // Act
        var elementString = cspElement
            .AddSource(specifiedSource)
            .Finish();

        // Assert
        Assert.Contains(specifiedSource, elementString);
    }

    [Fact]
    public void CspElementAddsHttpAndHttpsToSource()
    {
        // Arrange
        string specifiedSource = "specified-source";
        string specifiedSourceWithHttp = "http://" + specifiedSource;
        string specifiedSourceWithHttps = "https://" + specifiedSource;
        var cspElement = new ContentSecurityPolicyElement("sourceType");

        // Act
        var elementString = cspElement
            .AddSource(specifiedSource)
            .Finish();

        // Assert
        Assert.True(elementString.Contains(specifiedSourceWithHttp)
            && elementString.Contains(specifiedSourceWithHttps));
    }

    [Fact]
    public void CspElementDoesNotAddHttpAndHttpsToUnsafeInline()
    {
        // Arrange
        string unsafeInline = "'unsafe-inline'";
        string unsafeInlineWithHttp = "http://" + unsafeInline;
        string unsafeInlineWithHttps = "https://" + unsafeInline;
        var cspElement = new ContentSecurityPolicyElement("sourceType");

        // Act
        var elementString = cspElement
            .AddSource(unsafeInline)
            .Finish();

        // Assert
        Assert.True(elementString.Contains(unsafeInline)
            && !elementString.Contains(unsafeInlineWithHttp)
            && !elementString.Contains(unsafeInlineWithHttps));
    }

    [Fact]
    public void CspElementDoesNotAddHttpAndHttpsToUnsafeEval()
    {
        // Arrange
        string unsafeEval = "'unsafe-eval'";
        string unsafeEvalWithHttp = "http://" + unsafeEval;
        string unsafeEvalWithHttps = "https://" + unsafeEval;
        var cspElement = new ContentSecurityPolicyElement("sourceType");

        // Act
        var elementString = cspElement
            .AddSource(unsafeEval)
            .Finish();

        // Assert
        Assert.True(elementString.Contains(unsafeEval)
            && !elementString.Contains(unsafeEvalWithHttp)
            && !elementString.Contains(unsafeEvalWithHttps));
    }

    [Fact]
    public void CspElementDoesNotAddHttpAndHttpsToData()
    {
        // Arrange
        string data = "data:";
        string dataWithHttp = "http://" + data;
        string dataWithHttps = "https://" + data;
        var cspElement = new ContentSecurityPolicyElement("sourceType");

        // Act
        var elementString = cspElement
            .AddSource(data)
            .Finish();

        // Assert
        Assert.True(elementString.Contains(data)
            && !elementString.Contains(dataWithHttp)
            && !elementString.Contains(dataWithHttps));
    }

    [Fact]
    public void CspElementDoesNotAddHttpAndHttpsToSourceStartingWithWildcard()
    {
        // Arrange
        string wildcardSource = "*.source.com";
        string wildcardSourceWithHttp = "http://" + wildcardSource;
        string wildcardSourceWithHttps = "https://" + wildcardSource;
        var cspElement = new ContentSecurityPolicyElement("sourceType");

        // Act
        var elementString = cspElement
            .AddSource(wildcardSource)
            .Finish();

        // Assert
        Assert.True(elementString.Contains(wildcardSource)
            && !elementString.Contains(wildcardSourceWithHttp)
            && !elementString.Contains(wildcardSourceWithHttps));
    }

    [Fact]
    public void CspElementDoesNotAddHttpAndHttpsToHttps()
    {
        // Arrange
        string https = "https:";
        string httpsWithHttp = "http://" + https;
        string httpsWithHttps = "https://" + https;
        var cspElement = new ContentSecurityPolicyElement("sourceType");

        // Act
        var elementString = cspElement
            .AddSource(https)
            .Finish();

        // Assert
        Assert.True(elementString.Contains(https)
            && !elementString.Contains(httpsWithHttp)
            && !elementString.Contains(httpsWithHttps));
    }

    [Fact]
    public void CspElementDoesNotDuplicateHttpIfSourceAlreadyHasIt()
    {
        // Arrange
        string specifiedSource = "http://specified-source";
        string specifiedSourceWithExtraHttp = "http://" + specifiedSource;
        var cspElement = new ContentSecurityPolicyElement("sourceType");

        // Act
        var elementString = cspElement
            .AddSource(specifiedSource)
            .Finish();

        // Assert
        Assert.True(elementString.Contains(specifiedSource)
            && !elementString.Contains(specifiedSourceWithExtraHttp));
    }

    [Fact]
    public void CspElementDoesNotDuplicateHttpsIfSourceAlreadyHasIt()
    {
        // Arrange
        string specifiedSource = "https://specified-source";
        string specifiedSourceWithExtraHttps = "https://" + specifiedSource;
        var cspElement = new ContentSecurityPolicyElement("sourceType");

        // Act
        var elementString = cspElement
            .AddSource(specifiedSource)
            .Finish();

        // Assert
        Assert.True(elementString.Contains(specifiedSource)
            && !elementString.Contains(specifiedSourceWithExtraHttps));
    }

    [Fact]
    public void CspElementAddsHttpsIfSourceAlreadyHasHttp()
    {
        // Arrange
        string rawSource = "raw-source";
        string specifiedSource = "http://" + rawSource;
        string specifiedSourceWithHttpsInsteadOfHttp = "https://" + rawSource;
        var cspElement = new ContentSecurityPolicyElement("sourceType");

        // Act
        var elementString = cspElement
            .AddSource(specifiedSource)
            .Finish();

        // Assert
        Assert.True(elementString.Contains(specifiedSource)
            && elementString.Contains(specifiedSourceWithHttpsInsteadOfHttp));
    }

    [Fact]
    public void CspElementAddsHttpIfSourceAlreadyHasHttps()
    {
        // Arrange
        string rawSource = "raw-source";
        string specifiedSource = "https://" + rawSource;
        string specifiedSourceWithHttpInsteadOfHttps = "http://" + rawSource;
        var cspElement = new ContentSecurityPolicyElement("sourceType");

        // Act
        var elementString = cspElement
            .AddSource(specifiedSource)
            .Finish();

        // Assert
        Assert.True(elementString.Contains(specifiedSource)
            && elementString.Contains(specifiedSourceWithHttpInsteadOfHttps));
    }

    [Fact]
    public void CspElementEndsWithSemiColonAndSpace()
    {
        // Arrange
        var cspElement = new ContentSecurityPolicyElement("source-type");

        // Act
        var elementString = cspElement.Finish();

        // Assert
        Assert.EndsWith("; ", elementString);
    }

    [Fact]
    public void CspElementContainsAllPartsInCorrectOrder()
    {
        // Arrange
        string sourceType = "source-type";
        string sourcewithHttp = "http://source1.com";
        string sourcewithHttps = "https://source2.com";
        string plainSource = "source3.com";
        string sourceWithwildcard = "*.source4.com";
        string data = "data:";
        string https = "https:";
        string unsafeEval = "'unsafe-eval'";
        string unsafeInline = "'unsafe-inline'";

        // Act
        var elementString = new ContentSecurityPolicyElement(sourceType)
            .AddSource(sourcewithHttp)
            .AddSource(sourcewithHttps)
            .AddSource(plainSource)
            .AddSource(sourceWithwildcard)
            .AddSource(data)
            .AddSource(https)
            .AddSource(unsafeEval)
            .AddSource(unsafeInline)
            .Finish();

        // Assert
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
        Assert.Equal(expectedElement, elementString);
    }
}