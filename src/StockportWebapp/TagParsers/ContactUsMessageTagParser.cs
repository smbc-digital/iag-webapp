﻿namespace StockportWebapp.TagParsers;

public interface IContactUsMessageTagParser
{
    void Parse(IContactUsMessageContainer model, string message, string slug);
}

public class ContactUsMessageTagParser : IContactUsMessageTagParser
{
    private readonly IViewRender _viewRenderer;

    public ContactUsMessageTagParser(IViewRender viewRenderer)
    {
        _viewRenderer = viewRenderer;
    }

    public void Parse(IContactUsMessageContainer model, string message, string slug)
    {
        if (string.IsNullOrEmpty(message)) return;
        var htmlMessage = _viewRenderer.Render("ContactUsMessage", message);

        model.AddContactUsMessage(htmlMessage, slug);
    }
}