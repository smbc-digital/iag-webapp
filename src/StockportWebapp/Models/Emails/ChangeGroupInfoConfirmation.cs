namespace StockportWebapp.Models.Emails;

public class ChangeGroupInfoConfirmation
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public string Slug { get; set; }
}
