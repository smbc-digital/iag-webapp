namespace StockportWebapp.ContentFactory;

public static class ImageBlockAdapter
{
    public static ImageBlock FromJson(JsonElement obj)
    {
        var block = new ImageBlock();

        // altText
        if (obj.TryGetProperty("altText", out JsonElement altProp) &&
            altProp.ValueKind == JsonValueKind.String)
        {
            block.AltText = altProp.GetString()!;
        }

        // caption
        if (obj.TryGetProperty("caption", out JsonElement captionProp) &&
            captionProp.ValueKind == JsonValueKind.String)
        {
            block.Caption = captionProp.GetString()!;
        }

        // float
        if (obj.TryGetProperty("float", out JsonElement floatProp) &&
            floatProp.ValueKind == JsonValueKind.String)
        {
            block.Float = floatProp.GetString()!;
        }

        // image object
        if (obj.TryGetProperty("image", out JsonElement imageProp) &&
            imageProp.ValueKind == JsonValueKind.Object)
        {
            // image.fields.file.url
            if (imageProp.TryGetProperty("fields", out JsonElement fields) &&
                fields.TryGetProperty("file", out JsonElement file) &&
                file.TryGetProperty("url", out JsonElement urlProp) &&
                urlProp.ValueKind == JsonValueKind.String)
            {
                block.Image = urlProp.GetString()!;
            }
        }

        return block;
    }
}