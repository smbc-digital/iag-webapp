namespace StockportWebapp.TagParsers;

public interface IContactUsMessageTagParser
{
    void Parse(IContactUsMessageContainer model, string message, string slug);
}

public class ContactUsMessageTagParser(IViewRender viewRenderer) : IContactUsMessageTagParser
{
    private readonly IViewRender _viewRenderer = viewRenderer;

    public void Parse(IContactUsMessageContainer model, string message, string slug)
    {
        if (string.IsNullOrEmpty(message))
            return;

        model.AddContactUsMessage(_viewRenderer.Render("ContactUsMessage", message), slug);
    }
}