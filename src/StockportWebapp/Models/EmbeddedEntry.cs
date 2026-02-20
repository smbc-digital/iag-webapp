namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class EmbeddedEntry
{
    public ContentBlock? ContentBlock { get; set; }
    public ImageBlock? ImageBlock { get; set; }

    public bool IsContentBlock => ContentBlock != null;
    public bool IsImageBlock => ImageBlock != null;
}