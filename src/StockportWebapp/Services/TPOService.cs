namespace StockportWebapp.Services;

public interface ITPOService
{
	Task<TPOItem> GetTPODataByID(string heRef);
}

public class TPOService(ITPOApiClient tPOApiClient) : ITPOService
{
	private readonly ITPOApiClient _tPOApiClient = tPOApiClient;

	public async Task<TPOItem> GetTPODataByID(string heRef)
	{
		string json = await _tPOApiClient.GetTPODataByID(heRef);

		if (string.IsNullOrEmpty(json))
			return null;

		TPOItem tPOItem = System.Text.Json.JsonSerializer.Deserialize<TPOItem>(json);

		return tPOItem ?? null;
	}
}