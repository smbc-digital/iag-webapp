namespace StockportWebapp.Models.Emails;
[ExcludeFromCodeCoverage]
public class GroupArchiveWarningEmailViewModel(string administratorName, string groupName, string emailAddress)
{
    public string GroupName { get; set; } = groupName;
    public string AdministratorName { get; set; } = administratorName;
    public string EmailAddress { get; set; } = emailAddress;
}