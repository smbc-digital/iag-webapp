namespace StockportWebapp.Models.Config;

public class BusinessId(string businessId = "NOT SET")
{
    private string value = businessId;

    public override string ToString() =>
        value;

    public void SetId(string id) =>
        value = id;
}

public class BusinessIdFromRequest
{
    public string BusinessId { get; set; }
}