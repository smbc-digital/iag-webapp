namespace StockportWebappTests_Unit.Unit.Parsers;

public class VideoTagParserTests
{
    private readonly Mock<IViewRender> _viewRenderer = new();
    private readonly VideoTagParser _parser;

    public VideoTagParserTests() =>
        _parser = new VideoTagParser(_viewRenderer.Object);

    [Fact]
    public void ShouldParseTwentyThreeVideoTags()
    {
        // Act
        string response = _parser.Parse("{{VIDEO:VideoId;VideoToken}}");
        
        // Assert
        _viewRenderer.Verify(renderer => renderer.Render("VideoIFrame",
                                                        It.Is<VideoViewModel>(model => model.VideoToken.Equals("VideoToken")
                                                            && model.PhotoId.Equals("VideoId"))), Times.Once);
    }

    [Fact]
    public void ShouldParseTwentyThreeVideoTagsWithTitle()
    {
        // Act
        string response = _parser.Parse("{{VIDEO:VideoId;VideoToken;TestTitle}}");
        
        // Assert
        _viewRenderer.Verify(renderer => renderer.Render("VideoIFrame",
                                                        It.Is<VideoViewModel>(model => model.VideoToken.Equals("VideoToken")
                                                            && model.PhotoId.Equals("VideoId")
                                                            && model.Title.Equals("TestTitle"))), Times.Once);
    }
}