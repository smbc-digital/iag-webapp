﻿namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class TriviaFactoryTests
{
    private readonly MarkdownWrapper _markdownWrapper = new();

    private readonly TriviaFactory _factory;

    public TriviaFactoryTests()
    {
        _factory = new TriviaFactory(_markdownWrapper);
    }

    [Fact]
    public void Build_ShouldReturnCorrectListOfTrivia()
    {
        // Arrange
        Trivia triviaRequest = new("name", "icon", "body", "link");
        List<Trivia> request = new()
        {
            triviaRequest
        };

        // Act
        List<Trivia> result = _factory.Build(request);

        // Assert
        Assert.NotEmpty(result);
        Trivia firstResult = result.First();
        Assert.Equal(triviaRequest.Name, firstResult.Name);
        Assert.Equal(triviaRequest.Icon, firstResult.Icon);
        Assert.Equal("<p>body</p>\n", firstResult.Body);
        Assert.Equal(triviaRequest.Link, firstResult.Link);
    }

    [Fact]
    public void Build_ShouldReturnNullIfNoTrivia()
    {
        // Act
        List<Trivia> result = _factory.Build(null);

        // Assert
        Assert.Null(result);
    }
}