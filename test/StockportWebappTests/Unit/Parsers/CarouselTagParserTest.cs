namespace StockportWebappTests_Unit.Unit.Parsers;

public class CarouselTagParserTest
{
    private readonly CarouselTagParser _parser = new();

    [Fact]
    public void ShouldParseCarouselTagWithOneImage()
    {
        // Arrange
        string scriptTag = "<script>\r\nrequire(['/assets/javascript/config-70042ab6.min.js'],function(){\r\nrequire(['slick', 'carousel'],\r\nfunction(_, carousel){\r\ncarousel.Init();\r\n}\r\n);\r\n});\r\n</script>";

        // Act
        string response = _parser.Parse("{{CAROUSEL:![Frosty Twigs](Frosty_twigs_2.jpg)}}");

        // Assert
        Assert.Equal($"<div class='carousel'><div class=\"carousel-image stockport-carousel\" style=\"background-image:url(Frosty_twigs_2.jpg);\" title=\"Frosty Twigs\"><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">Frosty Twigs</p></div></div></div>{scriptTag}", response);
    }

    [Fact]
    public void ShouldParseCarouselTagWithMoreThanImage()
    {
        // Arrange
        string tag = "![UITEST:Image 1](//images.contentful.com/6cfgzlmcakf7/2FNKi2ZYcomigKGAqWiSyC/aec4f120903972fd6c842e84603c6a50/Flu_Jab.jpg), ![UITEST:Image 2](//images.contentful.com/6cfgzlmcakf7/3TsbpFmDewU4iuEAQU4IAo/afb6c155b46b9a0f196409ebb47cd4ff/Cllr_Foster_and_the_children_from_RECLAIM.jpg), ![UITEST:Image 3](//images.contentful.com/6cfgzlmcakf7/5NxhtngXYcEUUa6uOy6Ym6/a9263e313e538c45d04ce9a813880a2d/Art_gallery_image_2.jpg)";
        string scriptTag = "<script>\r\nrequire(['/assets/javascript/config-70042ab6.min.js'],function(){\r\nrequire(['slick', 'carousel'],\r\nfunction(_, carousel){\r\ncarousel.Init();\r\n}\r\n);\r\n});\r\n</script>";

        // Act
        string response = _parser.Parse($"{{{{CAROUSEL:![Frosty Twigs](Frosty_twigs_2.jpg),![Autumn Leaves](autumn_leaves.jpg){tag}}}}}");

        // Assert
        Assert.Equal($"<div class='carousel'><div class=\"carousel-image stockport-carousel\" style=\"background-image:url(Frosty_twigs_2.jpg);\" title=\"Frosty Twigs\"><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">Frosty Twigs</p></div></div><div class=\"carousel-image stockport-carousel\" style=\"background-image:url(autumn_leaves.jpg);\" title=\"Autumn Leaves\"><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">Autumn Leaves</p></div></div><div class=\"carousel-image stockport-carousel\" style=\"background-image:url(//images.contentful.com/6cfgzlmcakf7/3TsbpFmDewU4iuEAQU4IAo/afb6c155b46b9a0f196409ebb47cd4ff/Cllr_Foster_and_the_children_from_RECLAIM.jpg);\" title=\"UITEST:Image 2\"><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">UITEST:Image 2</p></div></div><div class=\"carousel-image stockport-carousel\" style=\"background-image:url(//images.contentful.com/6cfgzlmcakf7/5NxhtngXYcEUUa6uOy6Ym6/a9263e313e538c45d04ce9a813880a2d/Art_gallery_image_2.jpg);\" title=\"UITEST:Image 3\"><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">UITEST:Image 3</p></div></div></div>{scriptTag}", response);
    }

    [Fact]
    public void ShouldParseCarouselNoImages()
    {
        // Arrange
        string scriptTag = "<script>\r\nrequire(['/assets/javascript/config-70042ab6.min.js'],function(){\r\nrequire(['slick', 'carousel'],\r\nfunction(_, carousel){\r\ncarousel.Init();\r\n}\r\n);\r\n});\r\n</script>";

        // Act
        string response = _parser.Parse("{{CAROUSEL: }}");
       
        // Assert
        Assert.Equal($"<div class='carousel'></div>{scriptTag}", response);
    }

    [Fact]
    public void ShouldParseCarouselTagWithMoreThanImageButNoCommaBeforeLastImage()
    {
        // Arrange
        string scriptTag = "<script>\r\nrequire(['/assets/javascript/config-70042ab6.min.js'],function(){\r\nrequire(['slick', 'carousel'],\r\nfunction(_, carousel){\r\ncarousel.Init();\r\n}\r\n);\r\n});\r\n</script>";
        
        // Act
        string response = _parser.Parse($"{{{{CAROUSEL:" + "![Frosty Twigs](Frosty_twigs_2.jpg),![Autumn Leaves](autumn_leaves.jpg)![Hello](hello.jpg)" + "}}");
        
        // Assert
        Assert.Equal($"<div class='carousel'><div class=\"carousel-image stockport-carousel\" style=\"background-image:url(Frosty_twigs_2.jpg);\" title=\"Frosty Twigs\"><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">Frosty Twigs</p></div></div><div class=\"carousel-image stockport-carousel\" style=\"background-image:url(autumn_leaves.jpg);\" title=\"Autumn Leaves\"><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">Autumn Leaves</p></div></div></div>{scriptTag}", response );
    }
}