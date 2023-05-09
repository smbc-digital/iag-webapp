namespace StockportWebapp.Models.Config;

public class BusinessId
{
    private string value;

    public BusinessId(string businessId = "NOT SET")
    {
        value = businessId;
    }

    public override string ToString()
    {
        return value;
    }

    public void SetId(string id)
    {
        value = id;
    }
}

public class BusinessIdFromRequest
{
    public string BusinessId { get; set; }
}