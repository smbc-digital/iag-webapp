using System.Text.Encodings.Web;

namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class EmbeddedImagePartial : IHtmlContent
{
    private readonly ImageBlock _image;

    public EmbeddedImagePartial(ImageBlock image)
    {
        _image = image;
    }

    public void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        string url = _image.Image;
        string alt = _image.AltText;
        string caption = _image.Caption;
        string floatClass = _image.Float?.ToLower() switch
        {
            "left" => "image-left",
            "right" => "image-right",
            _ => string.Empty
        };

        // CASE 1: No float, no caption → just <img>
        if (string.IsNullOrEmpty(floatClass) && string.IsNullOrEmpty(caption))
        {
            writer.Write($@"<img src=""{url}"" alt=""{alt}"" />");
            return;
        }

        // CASE 2: Float but no caption
        if (!string.IsNullOrEmpty(floatClass) && string.IsNullOrEmpty(caption))
        {
            writer.Write($@"
            <figure class=""{floatClass}"">
                <p><img src=""{url}"" alt=""{alt}"" /></p>
            </figure>");
            return;
        }

        // CASE 3: Float + caption
        if (!string.IsNullOrEmpty(floatClass) && !string.IsNullOrEmpty(caption))
        {
            writer.Write($@"
            <figure class=""{floatClass}"">
                <p><img src=""{url}"" alt=""{alt}"" /></p>
                <figcaption>{caption}</figcaption>
            </figure>");
            return;
        }

        // CASE 4: Caption but no float (rare but possible)
        writer.Write($@"
        <figure>
            <p><img src=""{url}"" alt=""{alt}"" /></p>
            <figcaption>{caption}</figcaption>
        </figure>");
    }
}