namespace StockportWebapp.Models;

public class ContactUsId
{
    public string Name { get; }
    public string Slug { get; }
    public string EmailAddress { get; }
    public string SuccessPageButtonText { get; }
    public string SuccessPageReturnUrl { get; }

    public ContactUsId(string name, string slug, string emailAddress, string successPageButtonText, string successPageReturnUrl)
    {
        Name = name;
        Slug = slug;
        EmailAddress = emailAddress;
        SuccessPageButtonText = successPageButtonText;
        SuccessPageReturnUrl = successPageReturnUrl;
    }
}
