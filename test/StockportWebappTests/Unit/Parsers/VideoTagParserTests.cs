namespace StockportWebappTests_Unit.Unit.Parsers;

public class VideoTagParserTests
{
    private readonly Mock<IViewRender> _viewRenderer;
    private readonly VideoTagParser _parser;

    public VideoTagParserTests()
    {
        _viewRenderer = new Mock<IViewRender>();
        _parser = new VideoTagParser(_viewRenderer.Object);
    }

    [Fact]
    public void ShouldParseTwentyThreeVideoTags()
    {
        var tag = "VideoId;VideoToken";
        var response = _parser.Parse("{{VIDEO:" + tag + "}}");
        _viewRenderer.Verify(o => o.Render("VideoIFrame", It.Is<VideoViewModel>(model => model.VideoToken.Equals("VideoToken") && model.PhotoId.Equals("VideoId"))), Times.Once);
    }

    [Fact]
    public void ShouldParseTwentyThreeVideoTagsWithTitle()
    {
        var tag = "VideoId;VideoToken;TestTitle";
        var response = _parser.Parse("{{VIDEO:" + tag + "}}");
        _viewRenderer.Verify(o => o.Render("VideoIFrame", It.Is<VideoViewModel>(model => model.VideoToken.Equals("VideoToken") && model.PhotoId.Equals("VideoId") && model.Title.Equals("TestTitle"))), Times.Once);
    }
}
