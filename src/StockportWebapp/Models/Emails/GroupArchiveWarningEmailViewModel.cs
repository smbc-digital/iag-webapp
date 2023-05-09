namespace StockportWebapp.Models.Emails;

public class GroupArchiveWarningEmailViewModel
{
    public string GroupName { get; set; }
    public string AdministratorName { get; set; }
    public string EmailAddress { get; set; }

    public GroupArchiveWarningEmailViewModel(string administratorName, string groupName, string emailAddress)
    {
        AdministratorName = administratorName;
        GroupName = groupName;
        EmailAddress = emailAddress;
    }
}
