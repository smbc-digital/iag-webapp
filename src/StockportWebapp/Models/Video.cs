﻿using System.Diagnostics.CodeAnalysis;

namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Video
{
    public string Heading { get; set; }
    public string Text { get; set; }
    public string VideoEmbedCode { get; set; }

    public Video() { }

    public Video(string heading, string text, string videoEmbedCode)
    {
        Heading = heading;
        Text = text;
        VideoEmbedCode = videoEmbedCode;
    }
}
